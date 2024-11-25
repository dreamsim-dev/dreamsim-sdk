using System;
using com.unity3d.mediation;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class IronSourceBannerListener : IBannerListener
{
    public event Action<string> OnBannerAdLoaded;
    public event Action<string> OnBannerAdLoadFailed;
    public event Action<string> OnBannerAdDisplayed;
    public event Action<string> OnBannerAdDisplayFailed;
    public event Action<string> OnBannerAdClicked;
    public event Action<string> OnBannerAdCollapsed;
    public event Action<string> OnBannerAdExpanded;
    public event Action<string> OnBannerAdLeftApplication;

    private IMediationBridge _mediation;

  

    public void Init()
    {
        IronSourceBannerEvents.onAdLoadedEvent += HandleBannerAdLoaded;
        IronSourceBannerEvents.onAdLoadFailedEvent += HandleBannerAdLoadFailed;
        IronSourceBannerEvents.onAdClickedEvent += HandleBannerAdClicked;
        IronSourceBannerEvents.onAdScreenPresentedEvent += HandleBannerAdDisplayed;
        IronSourceBannerEvents.onAdScreenDismissedEvent += HandleBannerAdCollapsed;
        IronSourceBannerEvents.onAdLeftApplicationEvent += HandleBannerAdLeftApplication;

        DreamsimLogger.Log("Banner ads initialized");
    }

    public void Load(IBannerListener.BannerSize size = IBannerListener.BannerSize.Banner, IBannerListener.BannerPosition position = IBannerListener.BannerPosition.Bottom)
    {
        if (size == IBannerListener.BannerSize.Adaptive)
        {
            var ironSize = IronSourceBannerSize.SMART;
            ironSize.SetAdaptive(true);
            var ironPosition = ConvertPosition(position);
            IronSource.Agent.loadBanner(ironSize, ironPosition);
               
        }
        else
        {
            var ironSize = ConvertSize(size);  
            var ironPosition = ConvertPosition(position);
            IronSource.Agent.loadBanner(ironSize, ironPosition);
        }
            
        DreamsimLogger.Log("Banner loading requested");
    }

    public void Show()
    {
        IronSource.Agent.displayBanner();
        DreamsimLogger.Log("Banner is shown.");
    }

    public void Hide()
    {
        IronSource.Agent.hideBanner();
        DreamsimLogger.Log("Banner hidden.");
    }

    public void Destroy()
    {
        IronSource.Agent.destroyBanner();
        DreamsimLogger.Log("Banner destroyed.");
    }

    private static IronSourceBannerSize ConvertSize(IBannerListener.BannerSize size)
    {
        return size switch
        {
            IBannerListener.BannerSize.Banner => IronSourceBannerSize.BANNER,
            IBannerListener.BannerSize.Large => IronSourceBannerSize.LARGE,
            IBannerListener.BannerSize.MediumRectangle => IronSourceBannerSize.RECTANGLE,
            _ => IronSourceBannerSize.BANNER
        };
    }

    private static IronSourceBannerPosition ConvertPosition(IBannerListener.BannerPosition position)
    {
        return position switch
        {
            IBannerListener.BannerPosition.Top => IronSourceBannerPosition.TOP,
            IBannerListener.BannerPosition.Bottom => IronSourceBannerPosition.BOTTOM,
            _ => IronSourceBannerPosition.BOTTOM
        };
    }

    private void HandleBannerAdLoaded(IronSourceAdInfo adInfo) { OnBannerAdLoaded?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdLoadFailed(IronSourceError error)
    {
        OnBannerAdLoadFailed?.Invoke(error.getDescription());
    }

    private void HandleBannerAdClicked(IronSourceAdInfo adInfo) { OnBannerAdClicked?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdDisplayed(IronSourceAdInfo adInfo) { OnBannerAdDisplayed?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdCollapsed(IronSourceAdInfo adInfo) { OnBannerAdCollapsed?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdLeftApplication(IronSourceAdInfo adInfo)
    {
        OnBannerAdLeftApplication?.Invoke(adInfo.ToString());
    }


    public void SubscribeBannerAdLoaded(Action<string> onBannerAdLoaded) => OnBannerAdLoaded += onBannerAdLoaded;

    public void SubscribeBannerAdLoadFailed(Action<string> onBannerAdLoadFailed)
        => OnBannerAdLoadFailed += onBannerAdLoadFailed;

    public void SubscribeBannerAdDisplayed(Action<string> onBannerAdDisplayed)
        => OnBannerAdDisplayed += onBannerAdDisplayed;

    public void SubscribeBannerAdDisplayFailed(Action<string> onBannerAdDisplayFailed)
        => OnBannerAdDisplayFailed += onBannerAdDisplayFailed;

    public void SubscribeBannerAdClicked(Action<string> onBannerAdClicked) => OnBannerAdClicked += onBannerAdClicked;

    public void SubscribeBannerAdCollapsed(Action<string> onBannerAdCollapsed)
        => OnBannerAdCollapsed += onBannerAdCollapsed;

    public void SubscribeBannerAdExpanded(Action<string> onBannerAdExpanded)
        => OnBannerAdExpanded += onBannerAdExpanded;

    public void SubscribeBannerAdLeftApplication(Action<string> onBannerAdLeftApplication)
        => OnBannerAdLeftApplication += onBannerAdLeftApplication;
}
}