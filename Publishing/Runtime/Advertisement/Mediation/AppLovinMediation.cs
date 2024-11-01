#define DREAMSIM_USE_APPLOVIN
using System;

namespace Dreamsim.Publishing
{

#if DREAMSIM_USE_APPLOVIN

    public class AppLovinMediation : MediationBase, IMediationBridge
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
        
        private readonly string _adUnitId;

        public AppLovinMediation(string key) : base(key)
        {
            _adUnitId = key;
        }
        
        public void Init()
        {
            MaxSdk.InitializeSdk();
        }

        public void ValidateIntegration()
        {
            MaxSdk.IsInitialized();
        }

        public void InitiatingWithoutAdvertising() { throw new NotImplementedException(); }

        public void SetConsent(bool consent)
        {
            MaxSdk.SetHasUserConsent(consent);    
        }

        public void SetMetaData(string key, string value)
        {
            MaxSdk.SetExtraParameter(key, value); //TODO: не уверен что это именно то
        }

        public void OnApplicationPause(bool isPaused) { throw new NotImplementedException(); }

        public void SubscribeSdkInitializationCompleted(Action handle_SkdInitialized)
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += _ => { handle_SkdInitialized?.Invoke(); };
        }
        
        public void SubscribeImpressionDataReady() { throw new NotImplementedException(); }

        public void LoadRewardedVideo()
        {
            MaxSdk.LoadRewardedAd(_adUnitId);
        }

        public void ShowRewardedVideo(string adSource)
        {
            _adSource = adSource;
            
            if (!MaxSdk.IsRewardedAdReady(_adUnitId)) return;
            
            OnAdRequested?.Invoke(_adSource);
            MaxSdk.ShowRewardedAd(_adUnitId);
        }

        public void ShowRewardedVideo(string adSource, string placement)
        {
            _adSource = adSource;
            
            if (!MaxSdk.IsRewardedAdReady(_adUnitId)) return;
            
            OnAdRequested?.Invoke(_adSource);
            MaxSdk.ShowRewardedAd(_adUnitId, placement);
        }
        
        public bool IsRewardedVideoAvailable() { throw new NotImplementedException(); }
        public void SetManualLoadRewardedVideo(bool isOn) { throw new NotImplementedException(); }

        public void SubscribeAdOpened(Action<string, AdInfo> onAdOpened)
        {
            OnAdOpened = onAdOpened;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += Handle_OnAdOpened;
        }

        public void SubscribeAdClosed(Action<string> onAdClosed)
        {
            OnAdClosed = onAdClosed;
            
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Handle_OnAdClosed;
        }
        
        public void SubscribeAdAvailable(Action<bool> onAdAvailable) { throw new NotImplementedException(); }
        public void SubscribeAdUnavailable(Action<bool> onAdUnavailable) { throw new NotImplementedException(); }

        public void SubscribeAdLoadFailed(Action<string> onAdLoadFailed)
        {
            OnAdLoadFailed = onAdLoadFailed;
            
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += Handle_OnAdLoadFailed;
        }

        public void SubscribeAdShowFailed(Action<string> onAdShowFailed)
        {
            OnAdShowFailed = onAdShowFailed;
            
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += Handle_OnAdShowFailed;
        }

        public void SubscribeAdRewarded(Action<string> onAdRewarded)
        {
            OnAdCompleted = onAdRewarded;
            
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += Handle_OnAdRewarded;
        }

        public void SubscribeAdClicked(Action<string> onAdClicked)
        {
            OnAdClicked = onAdClicked;
            
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += Handle_OnAdClicked;
        }
        
        private void Handle_OnAdRewarded(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            OnAdCompleted?.Invoke(_adSource);
        }
        
        private void Handle_OnAdOpened(string adUnitId, MaxSdkBase.AdInfo adInfoMax)
        {
            var adInfo = new AdInfo(adInfoMax.NetworkName, adInfoMax.Revenue!, _placement, adInfoMax.AdUnitIdentifier);
            
            OnAdOpened?.Invoke(_adSource, adInfo);
        }
        
        private void Handle_OnAdClosed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LoadRewardedVideo();
            OnAdClosed?.Invoke(_adSource);
        }
        
        private void Handle_OnAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            _retryAttempt++;
            DreamsimLogger.LogError(errorInfo);
            DreamsimLogger.LogError("Rewarded video attempt " + _retryAttempt);
            OnAdLoadFailed?.Invoke(_adSource);
            LoadRewardedVideo();
        }
        
        private void Handle_OnAdShowFailed(string adUnitId, MaxSdkBase.ErrorInfo error, MaxSdkBase.AdInfo adInfo)
        {
            DreamsimLogger.LogError(adInfo.NetworkName);
            DreamsimLogger.LogError(error);
            OnAdShowFailed?.Invoke(_adSource);
            LoadRewardedVideo();
        }
        
        private void Handle_OnAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnAdClicked?.Invoke(_adSource);
            _retryAttempt = 0;
        }
    }

#endif

}