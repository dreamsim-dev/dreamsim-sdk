using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class Advertisement : MonoBehaviour
{
    public readonly RewardedVideoListener RewardedVideo = new();

    private static IMediationBridge _mediation;

    private bool _isInitialized;

    public static void ValidateIntegration() { _mediation.ValidateIntegration(); }

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
        _mediation = new AppLovinMediation(settings.AppLovin.SdkKey, settings.AppLovin.UnitId);
        #endif

        if (settings.UseRewardedVideo)
        {
            await InitMediationAsync(settings.UseRewardedVideo);
        }
        else
        {
            DreamsimLogger.LogError("No ad type used");
        }
    }

    private async UniTask InitMediationAsync(bool useRewardedVideo)
    {
        DreamsimLogger.Log("Mediator initialization started");
        _mediation.SubscribeSdkInitializationCompleted(Handle_SkdInitialized);
        var trackingEnabled = Analytics.TrackingEnabled;
        _mediation.SetConsent(trackingEnabled);
        DreamsimLogger.Log($"Mediator consent set to {trackingEnabled}");

        _mediation.SetMetaData("Facebook_IS_CacheFlag", "IMAGE");
        _mediation.SetMetaData("Meta_Mixed_Audience", "true");
        _mediation.SetMetaData("Vungle_coppa", "false");
        _mediation.SetMetaData("AdMob_TFCD", "false");
        _mediation.SetMetaData("AdMob_TFUA", "false");
        _mediation.SetMetaData("InMobi_AgeRestricted", "false");
        _mediation.SetMetaData("Mintegral_COPPA", "false");
        _mediation.SetMetaData("Chartboost_Coppa", "false");

        if (useRewardedVideo)
        {
            RewardedVideo.Init(_mediation);
        }

        _mediation.ImpressionDataReady();
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