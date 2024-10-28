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
        #if DREAMSIM_USE_IRONSOURCE
        _mediation = new IronSourceMediation(settings.LevelPlay.AppKey);
        await InitMediationAsync(settings.LevelPlay.UseRewardedVideo);
        #elif DREAMSIM_USE_APPLOVIN
        _mediation = new AppLovinMediation(settings.AppLovin.SdkKey, settings.AppLovin.UnitId);
        await InitMediationAsync(settings.AppLovin.UseRewardedVideo);
        #endif
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
        _mediation.SetMetaData("Chartboost_Coppa","false");
        
        if (useRewardedVideo)
        {
            RewardedVideo.Init(_mediation);
        }
        
        _mediation.ImpressionDataReady();

        var advertisingId = _mediation.GetAdvertiserId();
        DreamsimLogger.Log(string.IsNullOrEmpty(advertisingId)
            ? "Mediator initiating without advertising id"
            : $"Mediator initiating with advertising id ({advertisingId})");

        _mediation.Init();

        if (Application.isEditor)
        {
            Handle_SkdInitialized();
        }

        await UniTask.WaitUntil(() => _isInitialized);
    }

    private void OnApplicationPause(bool isPaused) { _mediation.OnApplicationPause(isPaused); }

    private void Handle_SkdInitialized()
    {
        DreamsimLogger.Log("Mediator initialized");
        RewardedVideo.Load();
        _isInitialized = true;
    }
}
}