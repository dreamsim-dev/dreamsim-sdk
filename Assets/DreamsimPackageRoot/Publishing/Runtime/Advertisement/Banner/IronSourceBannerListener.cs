using System;

namespace Dreamsim.Publishing
{
public class IronSourceBannerListener : IBannerListener
{
    public event Action<string> OnAdRequested;
    public event Action<string> OnAdLoaded;
    public event Action<string> OnAdLoadFailed;
    public event Action<string> OnAdDisplayed;
    public event Action<string> OnAdClicked;
    public event Action<string> OnAdDismissed;
    public event Action<string> OnAdLeftApplication;

    private string _adSource;

    public void Init()
    {
        IronSourceBannerEvents.onAdLoadedEvent += HandleBannerAdLoaded;
        IronSourceBannerEvents.onAdLoadFailedEvent += HandleBannerAdLoadFailed;
        IronSourceBannerEvents.onAdClickedEvent += HandleBannerAdClicked;
        IronSourceBannerEvents.onAdScreenPresentedEvent += HandleBannerAdDisplayed;
        IronSourceBannerEvents.onAdScreenDismissedEvent += HandleBannerAdDismissed;
        IronSourceBannerEvents.onAdLeftApplicationEvent += HandleBannerAdLeftApplication;

        DreamsimLogger.Log("Banner ads initialized");
    }

    public void Load(string adSource,
        BannerSize size = BannerSize.Default,
        BannerPosition position = BannerPosition.Bottom)
    {
        _adSource = adSource;

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

        OnAdRequested?.Invoke(_adSource);

        DreamsimLogger.Log("Banner loading requested.");
    }

    public void Show() { IronSource.Agent.displayBanner(); }

    public void Hide() { IronSource.Agent.hideBanner(); }

    public void Destroy() { IronSource.Agent.destroyBanner(); }

    private static IronSourceBannerSize ConvertSize(BannerSize size)
    {
        return size switch
        {
            BannerSize.Default => IronSourceBannerSize.BANNER,
            BannerSize.Large => IronSourceBannerSize.LARGE,
            BannerSize.MREC => IronSourceBannerSize.RECTANGLE,
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

    private void HandleBannerAdLoaded(IronSourceAdInfo adInfo)
    {
        DreamsimLogger.Log("Banner loaded");
        OnAdLoaded?.Invoke(_adSource);
    }

    private void HandleBannerAdLoadFailed(IronSourceError error)
    {
        DreamsimLogger.LogError("Banner load failed");
        DreamsimLogger.LogError(error.ToString());
        OnAdLoadFailed?.Invoke(_adSource);
    }

    private void HandleBannerAdClicked(IronSourceAdInfo adInfo)
    {
        DreamsimLogger.Log("Banner clicked");
        OnAdClicked?.Invoke(_adSource);
    }

    private void HandleBannerAdDisplayed(IronSourceAdInfo adInfo)
    {
        DreamsimLogger.Log("Banner shown");
        OnAdDisplayed?.Invoke(_adSource);
    }

    private void HandleBannerAdDismissed(IronSourceAdInfo adInfo)
    {
        DreamsimLogger.Log("Banner dismissed");
        OnAdDismissed?.Invoke(_adSource);
    }

    private void HandleBannerAdLeftApplication(IronSourceAdInfo adInfo)
    {
        DreamsimLogger.Log("Banner left application");
        OnAdLeftApplication?.Invoke(_adSource);
    }
}
}