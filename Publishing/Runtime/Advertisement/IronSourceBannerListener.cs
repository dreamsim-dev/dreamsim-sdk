using System;

namespace Dreamsim.Publishing
{
public class IronSourceBannerListener : IBannerListener
{
    public event Action<string> OnAdLoaded;
    public event Action<string> OnAdLoadFailed;
    public event Action<string> OnAdDisplayed;
    public event Action<string> OnAdDisplayFailed;
    public event Action<string> OnAdClicked;
    public event Action<string> OnAdCollapsed;
    public event Action<string> OnAdExpanded;
    public event Action<string> OnAdLeftApplication;

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

    public void Load(BannerSize size = BannerSize.Banner, BannerPosition position = BannerPosition.Bottom)
    {
        if (size == BannerSize.Adaptive)
        {
            var ironSourceSize = IronSourceBannerSize.SMART;
            ironSourceSize.SetAdaptive(true);
            var ironPosition = ConvertPosition(position);
            IronSource.Agent.loadBanner(ironSourceSize, ironPosition);
        }
        else
        {
            var ironSize = ConvertSize(size);
            var ironPosition = ConvertPosition(position);
            IronSource.Agent.loadBanner(ironSize, ironPosition);
        }

        DreamsimLogger.Log("Banner loading requested.");
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

    private static IronSourceBannerSize ConvertSize(BannerSize size)
    {
        return size switch
        {
            BannerSize.Banner => IronSourceBannerSize.BANNER,
            BannerSize.Large => IronSourceBannerSize.LARGE,
            BannerSize.MediumRectangle => IronSourceBannerSize.RECTANGLE,
            _ => IronSourceBannerSize.BANNER
        };
    }

    private static IronSourceBannerPosition ConvertPosition(BannerPosition position)
    {
        return position switch
        {
            BannerPosition.Top => IronSourceBannerPosition.TOP,
            BannerPosition.Bottom => IronSourceBannerPosition.BOTTOM,
            _ => IronSourceBannerPosition.BOTTOM
        };
    }

    private void HandleBannerAdLoaded(IronSourceAdInfo adInfo) { OnAdLoaded?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdLoadFailed(IronSourceError error) { OnAdLoadFailed?.Invoke(error.getDescription()); }

    private void HandleBannerAdClicked(IronSourceAdInfo adInfo) { OnAdClicked?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdDisplayed(IronSourceAdInfo adInfo) { OnAdDisplayed?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdCollapsed(IronSourceAdInfo adInfo) { OnAdCollapsed?.Invoke(adInfo.ToString()); }

    private void HandleBannerAdLeftApplication(IronSourceAdInfo adInfo)
    {
        OnAdLeftApplication?.Invoke(adInfo.ToString());
    }
}
}