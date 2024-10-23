using System;
using DevToDev.Analytics;

namespace Dreamsim.Publishing
{
    public class IronSourceMediation : MediationBase, IMediationBridge
    {
        public event Action OnAdReady;
        public event Action<string> OnAdRequested;
        public event Action<string> OnAdCompleted;
        public event Action<string> OnAdClosed;
        public event Action<string> OnAdLoadFailed;
        public event Action<string> OnAdShowFailed;
        public event Action<bool> OnAvailabilityChanged;
        public event Action<string, AdInfo> OnAdOpened;
        public event Action<string> OnAdClicked;
        
        public IronSourceMediation(string appKey) : base(appKey) { }

        public void Init()
        {
            IronSourceRewardedVideoEvents.onAdReadyEvent += Handle_OnAdReady;
            
            IronSource.Agent.init(_appKey);
        }

        public void ValidateIntegration()
        {
            IronSource.Agent.validateIntegration();
        }

        public string GetAdvertiserId()
        {
            return IronSource.Agent.getAdvertiserId();
        }

        public void SetConsent(bool consent)
        {
            IronSource.Agent.setConsent(consent);
        }

        public void SetMetaData(string key, string value)
        {
            IronSource.Agent.setMetaData(key, value);
        }

        public void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void SubscribeSdkInitializationCompleted(Action handle_SkdInitialized)
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += handle_SkdInitialized;
        }

        public void ImpressionDataReady()
        {
            IronSourceEvents.onImpressionDataReadyEvent += Handle_ImpressionDataReady;
        }

        public void LoadRewardedVideo()
        {
            IronSource.Agent.loadRewardedVideo();
            DreamsimLogger.Log("Ad loading started");
        }

        public void ShowRewardedVideo(string adSource, string placement)
        {
            _adSource = adSource;
            OnAdRequested?.Invoke(_adSource);
            IronSource.Agent.showRewardedVideo(placement);
        }

        public bool IsRewardedVideoAvailable()
        {
            return IronSource.Agent.isRewardedVideoAvailable();
        }

        public void SetManualLoadRewardedVideo(bool isOn)
        {
            IronSource.Agent.setManualLoadRewardedVideo(isOn);
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

        public void SubscribeAdOpened(Action<string, AdInfo> onAdOpened, string placement)
        {
            OnAdOpened = onAdOpened;
            _placement = placement;
            
            IronSourceRewardedVideoEvents.onAdOpenedEvent += Handle_OnAdOpened;
        }

        public void SubscribeAdClosed(Action<string> onAdClosed)
        {
            OnAdClosed = onAdClosed;
            
            IronSourceRewardedVideoEvents.onAdClosedEvent += Handle_OnAdClosed;
        }

        public void SubscribeAdAvailable(Action<bool> onAdAvailable)
        {
            OnAvailabilityChanged = onAdAvailable;
            
            IronSourceRewardedVideoEvents.onAdAvailableEvent += Handle_OnAdAvailable;
        }

        public void SubscribeAdUnavailable(Action<bool> onAdUnavailable)
        {
            OnAvailabilityChanged = onAdUnavailable;
            
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += Handle_OnAdUnavailable;
        }

        public void SubscribeAdLoadFailed(Action<string> onAdLoadFailed)
        {
            OnAdLoadFailed = onAdLoadFailed;
            
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Handle_OnAdLoadFailed;
        }

        public void SubscribeAdShowFailed(Action<string> onAdShowFailed)
        {
            OnAdShowFailed = onAdShowFailed;
            
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += Handle_OnAdShowFailed;
        }

        public void SubscribeAdRewarded(Action<string> onAdRewarded)
        {
            OnAdCompleted = onAdRewarded;
            
            IronSourceRewardedVideoEvents.onAdRewardedEvent += Handle_OnAdRewarded;
        }

        public void SubscribeAdClicked(Action<string> onAdClicked)
        {
            OnAdClicked = onAdClicked;
            
            IronSourceRewardedVideoEvents.onAdClickedEvent += Handle_OnAdClicked;
        }
        
        private void Handle_OnAdOpened(IronSourceAdInfo ironSourceAdInfo)
        {
            var adInfo = new AdInfo(ironSourceAdInfo.adNetwork,
                (double)ironSourceAdInfo.revenue!,
                _placement,
                ironSourceAdInfo.adUnit);
            
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

        private void Handle_OnAdReady(IronSourceAdInfo adInfo)
        {
            OnAdReady?.Invoke();
        }
    }
}