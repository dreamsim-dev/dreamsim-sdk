using System;

namespace Dreamsim.Publishing
{
    internal interface IMediationBridge
    {
        event Action OnAdReady;
        event Action<string> OnAdRequested;
        event Action<string> OnAdCompleted;
        event Action<string> OnAdClosed;
        event Action<string> OnAdLoadFailed;
        event Action<string> OnAdShowFailed;
        event Action<bool> OnAvailabilityChanged;
        event Action<string, AdInfo> OnAdOpened;
        event Action<string> OnAdClicked;
        
        void Init();
        
        void ValidateIntegration();

        string GetAdvertiserId();
        
        void SetConsent(bool consent);

        void SetMetaData(string key, string value);
        
        void OnApplicationPause(bool isPaused);

        void SubscribeSdkInitializationCompleted(Action handle_SkdInitialized);

        void ImpressionDataReady();

        void LoadRewardedVideo();
        
        void ShowRewardedVideo(string adSource, string placement);
        
        bool IsRewardedVideoAvailable();

        void SetManualLoadRewardedVideo(bool isOn);

        void SubscribeAdOpened(Action<string, AdInfo> onAdOpened, string placement);
        
        void SubscribeAdClosed(Action<string> onAdClosed);
        
        void SubscribeAdAvailable(Action<bool> onAdAvailable);
        
        void SubscribeAdUnavailable(Action<bool> onAdUnavailable);
        
        void SubscribeAdLoadFailed(Action<string> onAdLoadFailed);
        
        void SubscribeAdShowFailed(Action<string> onAdShowFailed);
        
        void SubscribeAdRewarded(Action<string> onAdRewarded);
        
        void SubscribeAdClicked(Action<string> onAdClicked);
    }
}