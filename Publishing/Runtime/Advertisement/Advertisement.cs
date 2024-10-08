using Cysharp.Threading.Tasks;
using DevToDev.Analytics;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class Advertisement : MonoBehaviour
{
    public readonly RewardedVideoListener RewardedVideo = new();

    private bool _isInitialized;

    public static void ValidateIntegration() { IronSource.Agent.validateIntegration(); }

    internal async UniTask InitAsync(Settings.AdvertisementSettings settings)
    {
        await InitLevelPlayAsync(settings.LevelPlay);
    }

    private async UniTask InitLevelPlayAsync(Settings.AdvertisementSettings.LevelPlaySettings settings)
    {
        DreamsimLogger.Log("LevelPlay initialization started");
        IronSourceEvents.onSdkInitializationCompletedEvent += Handle_LevelPlaySkdInitialized;
        var trackingEnabled = Analytics.TrackingEnabled;
        IronSource.Agent.setConsent(trackingEnabled);
        DreamsimLogger.Log($"LevelPlay consent set to {trackingEnabled}");

        IronSource.Agent.setMetaData("Facebook_IS_CacheFlag", "IMAGE");
        IronSource.Agent.setMetaData("Meta_Mixed_Audience", "true");
        IronSource.Agent.setMetaData("Vungle_coppa", "false");
        IronSource.Agent.setMetaData("AdMob_TFCD", "false");
        IronSource.Agent.setMetaData("AdMob_TFUA", "false");
        IronSource.Agent.setMetaData("InMobi_AgeRestricted", "false");
        IronSource.Agent.setMetaData("Mintegral_COPPA", "false");
        IronSource.Agent.setMetaData("Chartboost_Coppa","false");

        if (settings.UseRewardedVideo)
        {
            RewardedVideo.Init();
        }
        
        IronSourceEvents.onImpressionDataReadyEvent += Handle_ImpressionDataReady;

        var advertisingId = IronSource.Agent.getAdvertiserId();
        DreamsimLogger.Log(string.IsNullOrEmpty(advertisingId)
            ? "LevelPlay initiating without advertising id"
            : $"LevelPlay initiating with advertising id ({advertisingId})");

        IronSource.Agent.init(settings.AppKey);

        if (Application.isEditor)
        {
            Handle_LevelPlaySkdInitialized();
        }

        await UniTask.WaitUntil(() => _isInitialized);
    }

    private void OnApplicationPause(bool isPaused) { IronSource.Agent.onApplicationPause(isPaused); }

    private void Handle_LevelPlaySkdInitialized()
    {
        DreamsimLogger.Log("LevelPlay initialized");
        RewardedVideo.Load();
        _isInitialized = true;
    }

    private void Handle_ImpressionDataReady(IronSourceImpressionData impressionData)
    {
        if (impressionData?.revenue == null) return;

        Firebase.Analytics.Parameter[] adParameters =
        {
            new("ad_platform", "ironSource"),
            new("ad_source", impressionData.adNetwork),
            new("ad_unit_name", impressionData.instanceName),
            new("ad_format", impressionData.adUnit),
            new("currency", "USD"),
            new("value", impressionData.revenue.Value)
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", adParameters);
        DTDAnalytics.AdImpression(impressionData.adNetwork,
            impressionData.revenue.Value,
            impressionData.placement,
            impressionData.adUnit);
    }
}
}