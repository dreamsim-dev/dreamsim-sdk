using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevToDev.Analytics;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
public class Analytics : MonoBehaviour
{
    [SerializeField]
    private AppsFlyerManager _appsFlyerManager;

    [SerializeField]
    private DevToDevManager _devToDevManager;

    [SerializeField]
    private FirebaseManager _firebaseManager;

    [SerializeField]
    private NetworkReachabilityTracker _networkReachabilityTracker;

    private PurchaseValidator _purchaseValidator;

    public static string AdvertisingId { get; private set; }
    public static bool TrackingEnabled { get; private set; }

    internal AppsFlyerManager AppsFlyerManager => _appsFlyerManager;

    private static readonly List<IInternalAnalyticsLogger> Loggers = new()
    {
        new FacebookLogger(),
        new DevToDevLogger(),
        new AppsFlyerLogger(),
        new FirebaseLogger()
    };

    internal async UniTask InitAsync(string storeAppId,
        Settings.AnalyticsSettings settings,
        Settings.GDPRSettings gdprSettings)
    {
        _purchaseValidator = new PurchaseValidator(settings.PurchaseValidatorSlug);
        await ProcessConsentAsync(gdprSettings.GoogleMobileAdsTestDeviceHashedIds);
        _appsFlyerManager.Init(storeAppId, settings.AppsFlyer);
        _devToDevManager.Init(settings.DevToDev, _appsFlyerManager.GetAppsFlyerId());
        await _firebaseManager.InitAsync();
        _networkReachabilityTracker.Run();
        InitAdvertisementEvents();
        DreamsimLogger.Log("Analytics initialized");
    }

    private async UniTask ProcessConsentAsync(List<string> testDeviceHashedIds)
    {
        #if UNITY_IOS
        var consentFlow = new IOSConsentFlow();
        await consentFlow.ProcessAsync();
        #endif

        var googleConsentFlow = new GoogleConsentFlow();
        try
        {
            await googleConsentFlow.ProcessAsync(testDeviceHashedIds);
        }
        catch (Exception e)
        {
            DreamsimLogger.LogError("Failed to process Google Consent Flow");
            DreamsimLogger.LogException(e);
        }

        #if UNITY_IOS
        AdvertisingId = consentFlow.AdvertisingId;
        TrackingEnabled = consentFlow.TrackingEnabled && googleConsentFlow.TrackingEnabled;
        #elif UNITY_ANDROID
        AdvertisingId = googleConsentFlow.AdvertisingId;
        TrackingEnabled = googleConsentFlow.TrackingEnabled;
        #endif

        #if UNITY_IOS && !UNITY_EDITOR
        if (new Version(UnityEngine.iOS.Device.systemVersion).CompareTo(new Version("14.5")) != -1)
        {
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(TrackingEnabled);
            AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
        }
        #elif UNITY_ANDROID && !UNITY_EDITOR
        AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
        #endif

        DreamsimLogger.Log($"Analytics: AdvertisingId requested ({AdvertisingId})");
    }

    public void Log(string eventName) { CallLoggersAction(l => l.Log(eventName), $"Custom event ({eventName})"); }

    public void Log(string eventName, List<EventParam> eventParams)
    {
        CallLoggersAction(l => l.Log(eventName, eventParams), $"Custom event ({eventName})");
    }

    public async void LogPurchase(PurchaseEventArgs args)
    {
        var isValid = await _purchaseValidator.ValidateAsync(args);
        if (isValid) CallLoggersAction(l => l.LogPurchase(args), $"Purchase ({args.purchasedProduct.definition.id})");

        const string pref = "[Dreamsim].Purchasing.EverPurchased";
        var everPurchased = PlayerPrefs.GetInt(pref, 0) == 1;
        if (!everPurchased)
        {
            LogFirstPurchase(args);
            PlayerPrefs.SetInt(pref, 1);
        }
    }

    public void LogPurchaseInitiation(Product product)
    {
        CallLoggersAction(l => l.LogPurchaseInitiation(product), $"Purchase initiation ({product.definition.id})");
    }

    public void LogTutorialStart() { CallLoggersAction(l => l.LogTutorialStart(), "Tutorial started"); }

    public void LogTutorialSkipped() { CallLoggersAction(l => l.LogTutorialSkipped(), "Tutorial skipped"); }

    public void LogTutorialStepCompletion(int step)
    {
        CallLoggersAction(l => l.LogTutorialStepCompletion(step), $"Tutorial step ({step})");
    }

    public void LogTutorialCompletion() { CallLoggersAction(l => l.LogTutorialCompletion(), "Tutorial completed"); }

    public void LogLevelUp(int level) { CallLoggersAction(l => l.LogLevelUp(level), $"Level up ({level})"); }

    public void LogContentView(string contentId)
    {
        CallLoggersAction(l => l.LogContentView(contentId), $"Content view ({contentId})");
    }

    public void LogCrossPromoImpression(string appId, string campaign, List<EventParam> eventParams)
    {
        CallLoggersAction(l => l.LogCrossPromoImpression(appId, campaign, eventParams),
            $"Cross promo ({appId}, {campaign})");
    }

    internal void LogNetworkReachability(bool isReachable)
    {
        CallLoggersAction(l => l.LogNetworkReachability(isReachable), $"Network reachability ({isReachable})");
    }

    private void LogFirstPurchase(PurchaseEventArgs args)
    {
        CallLoggersAction(l => l.LogFirstPurchase(args), "First purchase");
    }

    private void LogRewardedAdRequest(string adSource)
    {
        CallLoggersAction(l => l.LogRewardedAdRequest(adSource), $"Ad requested ({adSource})");
    }

    private void LogRewardedAdImpression(string adSource)
    {
        CallLoggersAction(l => l.LogRewardedAdImpression(adSource), $"Ad impression ({adSource})");
    }

    private void LogRewardedAdClicked(string adSource)
    {
        CallLoggersAction(l => l.LogRewardedAdClicked(adSource), $"Ad clicked ({adSource})");
    }

    private void LogRewardedAdRewardReceived(string adSource)
    {
        CallLoggersAction(l => l.LogRewardedAdRewardReceived(adSource), $"Ad reward ({adSource})");

        const string pref = "[Dreamsim].Advertisement.RewardedVideo.TotalRewardsReceived";
        const int interval = 30;
        var total = PlayerPrefs.GetInt(pref, 0) + 1;
        if (total % interval == 0)
        {
            LogRewardedAdRewardReceivedTimes(interval);
        }

        PlayerPrefs.SetInt(pref, total);
    }

    private void LogRewardedAdRewardReceivedTimes(int times)
    {
        CallLoggersAction(l => l.LogRewardedAdRewardReceivedTimes(times),
            $"Reward received {times} times");
    }

    private void CallLoggersAction(Action<IInternalAnalyticsLogger> action, string logMsg)
    {
        if (!Application.isEditor) Loggers.ForEach(action);
        DreamsimLogger.Log($"Analytics event triggered: {logMsg}");
    }

    private void InitAdvertisementEvents()
    {
        DreamsimPublishing.Advertisement.RewardedVideo.OnAdRequested += OnRewardedAdRequested;
        DreamsimPublishing.Advertisement.RewardedVideo.OnAdOpened += OnRewardedAdOpened;
        DreamsimPublishing.Advertisement.RewardedVideo.OnAdClicked += OnRewardedAdClicked;
        DreamsimPublishing.Advertisement.RewardedVideo.OnAdCompleted += OnRewardedAdCompleted;
        DreamsimPublishing.Advertisement.RewardedVideo.OnImpressionDataReady += OnRewardedAdImpressionDataReady;
    }

    private void OnRewardedAdRequested(string adSource) { LogRewardedAdRequest(adSource); }

    private void OnRewardedAdOpened(string adSource, AdInfo adInfo) { LogRewardedAdImpression(adSource); }

    private void OnRewardedAdClicked(string adSource) { LogRewardedAdClicked(adSource); }

    private void OnRewardedAdCompleted(string adSource) { LogRewardedAdRewardReceived(adSource); }

    private static void OnRewardedAdImpressionDataReady(ImpressionData impressionData)
    {
        if (Application.isEditor) return;
        if (impressionData.Revenue == null) return;

        Firebase.Analytics.Parameter[] adParameters =
        {
            new("ad_platform", "ironSource"),
            new("ad_source", impressionData.AdNetwork),
            new("ad_unit_name", impressionData.InstanceName),
            new("ad_format", impressionData.AdUnit),
            new("currency", "USD"),
            new("value", impressionData.Revenue.Value)
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", adParameters);

        DTDAnalytics.AdImpression(impressionData.AdNetwork,
            impressionData.Revenue.Value,
            impressionData.Placement,
            impressionData.AdUnit);
    }
}
}