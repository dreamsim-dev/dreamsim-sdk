using System;

namespace Dreamsim.Publishing
{
public class RewardedVideoListener
{
    public event Action<string> OnAdRequested;
    public event Action<string> OnAdCompleted;
    public event Action<string> OnAdClosed;
    public event Action<string> OnAdLoadFailed;
    public event Action<string> OnAdShowFailed;
    public event Action<bool> OnAvailabilityChanged;
    public event Action<string, AdInfo> OnAdOpened;
    public event Action<string> OnAdClicked;
    public event Action<ImpressionData> OnImpressionDataReady;

    private IMediationBridge _mediation;

    public bool IsAvailable => _mediation?.IsRewardedVideoAvailable() ?? false;

    internal void Init(IMediationBridge mediator)
    {
        _mediation = mediator;

        _mediation.SetManualLoadRewardedVideo(true);

        _mediation.OnAdReady += () => DreamsimLogger.Log("Rewarded video ready");
        _mediation.OnAdShowFailed += _ => DreamsimLogger.LogError("Rewarded video show failed");
        _mediation.OnAdRequested += adSource => OnAdRequested?.Invoke(adSource);
        
        _mediation.SubscribeAdOpened((adSource, adInfo) => OnAdOpened?.Invoke(adSource, adInfo));
        _mediation.SubscribeAdClosed(adSource => OnAdClosed?.Invoke(adSource));
        _mediation.SubscribeAdAvailable(adSource => OnAvailabilityChanged?.Invoke(adSource));
        _mediation.SubscribeAdUnavailable(adSource => OnAvailabilityChanged?.Invoke(adSource));
        _mediation.SubscribeAdLoadFailed(adSource => OnAdLoadFailed?.Invoke(adSource));
        _mediation.SubscribeAdShowFailed(adSource => OnAdShowFailed?.Invoke(adSource));
        _mediation.SubscribeAdRewarded(adSource => OnAdCompleted?.Invoke(adSource));
        _mediation.SubscribeAdClicked(adSource => OnAdClicked?.Invoke(adSource));
        _mediation.SubscribeImpressionDataReady(data => OnImpressionDataReady?.Invoke(data));

        DreamsimLogger.Log("Rewarded ads initialized");
    }

    public void Show(string adSource) { _mediation.ShowRewardedVideo(adSource); }

    public void Show(string adSource, string placement) { _mediation.ShowRewardedVideo(adSource, placement); }

    internal void Load() { _mediation.LoadRewardedVideo(); }
}
}