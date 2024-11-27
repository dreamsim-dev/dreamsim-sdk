using System;

namespace Dreamsim.Publishing
{
public interface IBannerListener
{
    event Action<string> OnAdLoaded;
    event Action<string> OnAdLoadFailed;
    event Action<string> OnAdDisplayed;
    event Action<string> OnAdDisplayFailed;
    event Action<string> OnAdClicked;
    event Action<string> OnAdCollapsed;
    event Action<string> OnAdExpanded;
    event Action<string> OnAdLeftApplication;

    void Init();

    void Load(BannerSize size, BannerPosition position);

    void Show();

    void Hide();

    void Destroy();
}
}