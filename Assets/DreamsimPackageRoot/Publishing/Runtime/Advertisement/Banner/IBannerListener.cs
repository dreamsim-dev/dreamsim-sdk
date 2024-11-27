using System;

namespace Dreamsim.Publishing
{
public interface IBannerListener
{
    event Action<string> OnAdRequested;
    event Action<string> OnAdLoaded;
    event Action<string> OnAdLoadFailed;
    event Action<string> OnAdDisplayed;
    event Action<string> OnAdClicked;
    event Action<string> OnAdDismissed;
    event Action<string> OnAdLeftApplication;

    void Init();

    void Load(string adSource,
        BannerSize size = BannerSize.Default,
        BannerPosition position = BannerPosition.Bottom);

    void Show();

    void Hide();

    void Destroy();
}
}