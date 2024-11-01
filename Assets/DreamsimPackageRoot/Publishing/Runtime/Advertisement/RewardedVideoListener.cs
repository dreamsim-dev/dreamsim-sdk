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

    private IMediationBridge _mediation;

    public bool IsAvailable => _mediation?.IsRewardedVideoAvailable() ?? false;

    internal void Init(IMediationBridge mediator)
    {
        _mediation = mediator;

        _mediation.SetManualLoadRewardedVideo(true);

        _mediation.OnAdReady += () => DreamsimLogger.Log("Rewarded video ready");
        _mediation.OnAdShowFailed += _ => DreamsimLogger.LogError("Rewarded video show failed");

        _mediation.SubscribeAdOpened(OnAdOpened, DefaultPlacement);
        _mediation.SubscribeAdClosed(OnAdClosed);
        _mediation.SubscribeAdAvailable(OnAvailabilityChanged);
        _mediation.SubscribeAdUnavailable(OnAvailabilityChanged);
        _mediation.SubscribeAdLoadFailed(OnAdLoadFailed);
        _mediation.SubscribeAdShowFailed(OnAdShowFailed);
        _mediation.SubscribeAdRewarded(OnAdCompleted);
        _mediation.SubscribeAdClicked(OnAdClicked);

        DreamsimLogger.Log("Rewarded ads initialized");
    }

    public void Show(string adSource, string placement = DefaultPlacement)
    {
        _mediation.ShowRewardedVideo(adSource, placement);
    }

    internal void Load() { _mediation.LoadRewardedVideo(); }
}
}