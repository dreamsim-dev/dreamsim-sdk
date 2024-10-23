using System;

namespace Dreamsim.Publishing
{
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
        
        public AppLovinMediation(string appKey) : base(appKey) { }
        
        public void Init() { throw new NotImplementedException(); }
        public void ValidateIntegration() { throw new NotImplementedException(); }
        public string GetAdvertiserId() { throw new NotImplementedException(); }
        public void SetConsent(bool consent) { throw new NotImplementedException(); }
        public void SetMetaData(string key, string value) { throw new NotImplementedException(); }
        public void OnApplicationPause(bool isPaused) { throw new NotImplementedException(); }
        public void SubscribeSdkInitializationCompleted(Action handle_SkdInitialized) { throw new NotImplementedException(); }
        public void ImpressionDataReady() { throw new NotImplementedException(); }
        public void LoadRewardedVideo() { throw new NotImplementedException(); }
        public void ShowRewardedVideo(string adSource, string placement) { throw new NotImplementedException(); }
        public bool IsRewardedVideoAvailable() { throw new NotImplementedException(); }
        public void SetManualLoadRewardedVideo(bool isOn) { throw new NotImplementedException(); }
        public void SubscribeAdOpened(Action<string, AdInfo> onAdOpened, string placement) { throw new NotImplementedException(); }
        public void SubscribeAdClosed(Action<string> onAdClosed) { throw new NotImplementedException(); }
        public void SubscribeAdAvailable(Action<bool> onAdAvailable) { throw new NotImplementedException(); }
        public void SubscribeAdUnavailable(Action<bool> onAdUnavailable) { throw new NotImplementedException(); }
        public void SubscribeAdLoadFailed(Action<string> onAdLoadFailed) { throw new NotImplementedException(); }
        public void SubscribeAdShowFailed(Action<string> onAdShowFailed) { throw new NotImplementedException(); }
        public void SubscribeAdRewarded(Action<string> onAdRewarded) { throw new NotImplementedException(); }
        public void SubscribeAdClicked(Action<string> onAdClicked) { throw new NotImplementedException(); }
    }
}