using System;
using UnityEngine;

namespace Dreamsim.Publishing
{
#if DREAMSIM_USE_IRONSOURCE
public class IronSourceMediation : MediationBase, IMediationBridge
{
    private const string DefaultPlacement = "DefaultRewardedVideo";

    public event Action OnAdReady;
    public event Action<string> OnAdRequested;
    public event Action<string> OnAdCompleted;
    public event Action<string> OnAdClosed;
    public event Action<string> OnAdLoadFailed;
    public event Action<string> OnAdShowFailed;
    public event Action<bool> OnAvailabilityChanged;
    public event Action<string, AdInfo> OnAdOpened;
    public event Action<string> OnAdClicked;
    public event Action<ImpressionData> OnImpressionDataReady;

    private readonly string _appKey;

    public IronSourceMediation(string key)
        : base(key)
    {
        _appKey = key;
    }

    public void Init()
    {
        IronSourceRewardedVideoEvents.onAdReadyEvent += Handle_OnAdReady;

        IronSource.Agent.init(_appKey);
    }

    public void ValidateIntegration() { IronSource.Agent.validateIntegration(); }

    public void InitiatingWithoutAdvertising()
    {
        var advertisingId = IronSource.Agent.getAdvertiserId();
        DreamsimLogger.Log(string.IsNullOrEmpty(advertisingId)
            ? "IronSource initiating without advertising id"
            : $"IronSource initiating with advertising id ({advertisingId})");
    }

    public void SetMetaData(string key, string value)
    {
        IronSource.Agent.setMetaData(key, value);
    }

    public void SetConsent(bool consent) { IronSource.Agent.setConsent(consent); }
    
    public void SetCOPPA(bool value) 
    {
        IronSource.Agent.setMetaData("Vungle_coppa", value.ToString());
        IronSource.Agent.setMetaData("AdMob_TFCD", value.ToString());
        IronSource.Agent.setMetaData("AdMob_TFUA", value.ToString());
        IronSource.Agent.setMetaData("InMobi_AgeRestricted", value.ToString());
        IronSource.Agent.setMetaData("Mintegral_COPPA", value.ToString());
        IronSource.Agent.setMetaData("Chartboost_Coppa", value.ToString());
    }

    public void OnApplicationPause(bool isPaused)
    {
        #if !UNITY_EDITOR
        IronSource.Agent.onApplicationPause(isPaused);
        #endif
    }

    public void SubscribeSdkInitializationCompleted(Action action)
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += action;
    }

    public void SubscribeImpressionDataReady(Action<ImpressionData> onImpressionDataReady)
    {
        OnImpressionDataReady += onImpressionDataReady;
        IronSourceEvents.onImpressionDataReadyEvent += Handle_ImpressionDataReady;
    }

    public void LoadRewardedVideo()
    {
        if (!Application.isEditor)
        {
            IronSource.Agent.loadRewardedVideo();
        }

        DreamsimLogger.Log("Ad loading started");
    }

    public void ShowRewardedVideo(string adSource)
    {
        ShowRewardedVideo(adSource, DefaultPlacement);
    }

    public void ShowRewardedVideo(string adSource, string placement)
    {
        _adSource = adSource;
        OnAdRequested?.Invoke(_adSource);

        if (Application.isEditor)
        {
            Handle_OnAdOpened(null);
            Handle_OnAdClicked(null, null);
            Handle_OnAdRewarded(null, null);
            Handle_OnAdClosed(null);
        }
        else
        {
            IronSource.Agent.showRewardedVideo(placement);
        }
    }

    public bool IsRewardedVideoAvailable() { return IronSource.Agent.isRewardedVideoAvailable(); }

    public void SetManualLoadRewardedVideo(bool isOn) { IronSource.Agent.setManualLoadRewardedVideo(isOn); }

    private void Handle_ImpressionDataReady(IronSourceImpressionData impressionData)
    {
        var impression = new ImpressionData(
            impressionData.auctionId,
            impressionData.adUnit,
            impressionData.country,
            impressionData.ab,
            impressionData.segmentName,
            impressionData.placement,
            impressionData.adNetwork,
            impressionData.instanceName,
            impressionData.instanceId,
            impressionData.revenue,
            impressionData.precision,
            impressionData.lifetimeRevenue,
            impressionData.encryptedCPM,
            impressionData.conversionValue,
            impressionData.allData);
        
        OnImpressionDataReady?.Invoke(impression);
    }

    public void SubscribeAdOpened(Action<string, AdInfo> onAdOpened)
    {
        OnAdOpened += onAdOpened;
        IronSourceRewardedVideoEvents.onAdOpenedEvent += Handle_OnAdOpened;
    }

    public void SubscribeAdClosed(Action<string> onAdClosed)
    {
        OnAdClosed += onAdClosed;
        IronSourceRewardedVideoEvents.onAdClosedEvent += Handle_OnAdClosed;
    }

    public void SubscribeAdAvailable(Action<bool> onAdAvailable)
    {
        OnAvailabilityChanged += onAdAvailable;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += Handle_OnAdAvailable;
    }

    public void SubscribeAdUnavailable(Action<bool> onAdUnavailable)
    {
        OnAvailabilityChanged += onAdUnavailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += Handle_OnAdUnavailable;
    }

    public void SubscribeAdLoadFailed(Action<string> onAdLoadFailed)
    {
        OnAdLoadFailed += onAdLoadFailed;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Handle_OnAdLoadFailed;
    }

    public void SubscribeAdShowFailed(Action<string> onAdShowFailed)
    {
        OnAdShowFailed += onAdShowFailed;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += Handle_OnAdShowFailed;
    }

    public void SubscribeAdRewarded(Action<string> onAdRewarded)
    {
        OnAdCompleted += onAdRewarded;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += Handle_OnAdRewarded;
    }

    public void SubscribeAdClicked(Action<string> onAdClicked)
    {
        OnAdClicked += onAdClicked;
        IronSourceRewardedVideoEvents.onAdClickedEvent += Handle_OnAdClicked;
    }

    private void Handle_OnAdOpened(IronSourceAdInfo ironSourceAdInfo)
    {
        AdInfo adInfo;
        if (Application.isEditor)
        {
            adInfo = new AdInfo();
        }
        else
        {
            adInfo = new AdInfo(ironSourceAdInfo?.adNetwork,
                (double)ironSourceAdInfo!.revenue!,
                _placement,
                ironSourceAdInfo.adUnit);
        }

        OnAdOpened?.Invoke(_adSource, adInfo);
    }

    private void Handle_OnAdClosed(IronSourceAdInfo adInfo)
    {
        LoadRewardedVideo();
        OnAdClosed?.Invoke(_adSource);
    }

    private void Handle_OnAdAvailable(IronSourceAdInfo adInfo)
    {
        _retryAttempt = 0;
        DreamsimLogger.Log($"Rewarded video loaded ({adInfo.adNetwork})");
        OnAvailabilityChanged?.Invoke(true);
    }

    private void Handle_OnAdUnavailable()
    {
        DreamsimLogger.Log("Rewarded video unavailable");
        OnAvailabilityChanged?.Invoke(false);
    }

    private void Handle_OnAdLoadFailed(IronSourceError error)
    {
        _retryAttempt++;
        DreamsimLogger.LogError(error);
        DreamsimLogger.LogError("Rewarded video attempt " + _retryAttempt);
        OnAdLoadFailed?.Invoke(_adSource);
        LoadRewardedVideo();
    }

    private void Handle_OnAdShowFailed(IronSourceError error, IronSourceAdInfo adInfo)
    {
        DreamsimLogger.LogError(adInfo.adNetwork);
        DreamsimLogger.LogError(error);
        OnAdShowFailed?.Invoke(_adSource);
        LoadRewardedVideo();
    }

    private void Handle_OnAdRewarded(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        OnAdCompleted?.Invoke(_adSource);
    }

    private void Handle_OnAdClicked(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        OnAdClicked?.Invoke(_adSource);
        _retryAttempt = 0;
    }

    private void Handle_OnAdReady(IronSourceAdInfo adInfo) { OnAdReady?.Invoke(); }
}
#endif
}