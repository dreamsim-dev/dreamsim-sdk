using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class Advertisement : MonoBehaviour
{
    public event Action<ImpressionData> OnImpressionDataReady;

    public readonly RewardedVideoListener RewardedVideo;
    public readonly IBannerListener Banner;

    private IMediationBridge _mediation;

    private bool _isInitialized;

    public void ValidateIntegration() { _mediation.ValidateIntegration(); }

    public Advertisement()
    {
        RewardedVideo = new RewardedVideoListener();
        #if DREAMSIM_USE_IRONSOURCE
        Banner = new IronSourceBannerListener();
        #endif
    }

    internal async UniTask InitAsync(Settings.AdvertisementSettings settings)
    {
        DreamsimLogger.Log("Advertisement initialization started");

        #if !DREAMSIM_USE_IRONSOURCE && !DREAMSIM_USE_APPLOVIN
        DreamsimLogger.LogError("No mediator selected. Advertisement initialization won't continue");
        #if !UNITY_EDITOR
        return;
        #endif
        #endif

        #if DREAMSIM_USE_IRONSOURCE
        _mediation = new IronSourceMediation(settings.LevelPlay.AppKey);
        #elif DREAMSIM_USE_APPLOVIN
        _mediation = new AppLovinMediation(settings.AppLovin.UnitId);
        #endif

        if (settings.UseRewardedVideo || settings.UseBanner)
        {
            await InitMediationAsync(settings.UseRewardedVideo, settings.UseBanner);
        }
        else
        {
            DreamsimLogger.LogError("No ad type used");
        }
    }

    private async UniTask InitMediationAsync(bool useRewardedVideo, bool useBannerAds)
    {
        DreamsimLogger.Log("Mediator initialization started");
        _mediation.SubscribeSdkInitializationCompleted(Handle_SkdInitialized);
        var trackingEnabled = Analytics.TrackingEnabled;
        _mediation.SetConsent(trackingEnabled);
        DreamsimLogger.Log($"Mediator consent set to {trackingEnabled}");

        #if DREAMSIM_USE_IRONSOURCE
        _mediation.SetMetaData("Facebook_IS_CacheFlag", "IMAGE");
        _mediation.SetMetaData("Meta_Mixed_Audience", "true");
        #endif

        _mediation.SetCOPPA(false);

        if (useRewardedVideo) RewardedVideo.Init(_mediation);
        if (useBannerAds) Banner.Init();

        _mediation.SubscribeImpressionDataReady(data => OnImpressionDataReady?.Invoke(data));

        _mediation.InitiatingWithoutAdvertising();
        _mediation.Init();

        if (Application.isEditor)
        {
            Handle_SkdInitialized();
        }

        await UniTask.WaitUntil(() => _isInitialized);
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (_isInitialized) _mediation.OnApplicationPause(isPaused);
    }

    private void Handle_SkdInitialized()
    {
        DreamsimLogger.Log("Mediator initialized");

        RewardedVideo.Load();
        _isInitialized = true;

        DreamsimLogger.Log("Advertisement initialized");
    }
}
}