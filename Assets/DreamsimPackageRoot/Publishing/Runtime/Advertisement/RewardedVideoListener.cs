using System;

namespace Dreamsim.Publishing
{
public class RewardedVideoListener
{
    private const string DefaultPlacement = "DefaultRewardedVideo";

    public event Action<string> OnAdRequested;
    public event Action<string> OnAdCompleted;
    public event Action<string> OnAdClosed;
    public event Action<string> OnAdLoadFailed;
    public event Action<string> OnAdShowFailed;
    public event Action<bool> OnAvailabilityChanged;
    public event Action<string, AdInfo> OnAdOpened;
    public event Action<string> OnAdClicked;

    private int _retryAttempt;
    private string _adSource;

    public bool IsAvailable => IronSource.Agent.isRewardedVideoAvailable();

    internal void Init()
    {
        IronSource.Agent.setManualLoadRewardedVideo(true);

        IronSourceRewardedVideoEvents.onAdReadyEvent += _ => DreamsimLogger.Log("Rewarded video ready");
        IronSourceRewardedVideoEvents.onAdShowFailedEvent +=
            (_, _) => DreamsimLogger.LogError("Rewarded video show failed");

        IronSourceRewardedVideoEvents.onAdOpenedEvent += Handle_OnAdOpened;
        IronSourceRewardedVideoEvents.onAdClosedEvent += Handle_OnAdClosed;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += Handle_OnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += Handle_OnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent += Handle_OnAdLoadFailed;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += Handle_OnAdShowFailed;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += Handle_OnAdRewarded;
        IronSourceRewardedVideoEvents.onAdClickedEvent += Handle_OnAdClicked;

        DreamsimLogger.Log("Rewarded ads initialized");
    }

    public void Show(string adSource, string placement = DefaultPlacement)
    {
        _adSource = adSource;
        OnAdRequested?.Invoke(_adSource);
        IronSource.Agent.showRewardedVideo(placement);
    }

    internal void Load()
    {
        IronSource.Agent.loadRewardedVideo();
        DreamsimLogger.Log("Ad loading started");
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

    private void Handle_OnAdClicked(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        OnAdClicked?.Invoke(_adSource);
        _retryAttempt = 0;
    }

    private void Handle_OnAdLoadFailed(IronSourceError error)
    {
        _retryAttempt++;
        DreamsimLogger.LogError(error);
        DreamsimLogger.LogError("Rewarded video attempt " + _retryAttempt);
        OnAdLoadFailed?.Invoke(_adSource);
        Load();
    }

    private void Handle_OnAdOpened(IronSourceAdInfo ironSourceAdInfo)
    {
        var adInfo = new AdInfo(ironSourceAdInfo.adNetwork,
            (double)ironSourceAdInfo.revenue!,
            DefaultPlacement,
            ironSourceAdInfo.adUnit);
        
        OnAdOpened?.Invoke(_adSource, adInfo);
    }

    private void Handle_OnAdShowFailed(IronSourceError error, IronSourceAdInfo adInfo)
    {
        DreamsimLogger.LogError(adInfo.adNetwork);
        DreamsimLogger.LogError(error);
        OnAdShowFailed?.Invoke(_adSource);
        Load();
    }

    private void Handle_OnAdRewarded(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        OnAdCompleted?.Invoke(_adSource);
    }

    private void Handle_OnAdClosed(IronSourceAdInfo adInfo)
    {
        Load();
        OnAdClosed?.Invoke(_adSource);
    }
}
}