using System;

namespace Dreamsim.Publishing
{
public  interface IBannerListener
{
    event Action<string> OnBannerAdLoaded;
    event Action<string> OnBannerAdLoadFailed;
    event Action<string> OnBannerAdDisplayed;
    event Action<string> OnBannerAdDisplayFailed;
    event Action<string> OnBannerAdClicked;
    event Action<string> OnBannerAdCollapsed;
    event Action<string> OnBannerAdExpanded;
    event Action<string> OnBannerAdLeftApplication;
   
    public enum BannerSize
    {
        Banner, // Standard banner 320 x 50
        Large, // Large banner 320 x 90
        MediumRectangle, // Medium Rectangular (MREC) 300 x 250
        Adaptive // Automatically adjusts size and orientation for mobile & tablets.
    }

    public enum BannerPosition
    {
        Top,
        Bottom
    }
    
    void Init();
    void Load(BannerSize size, BannerPosition position);
    void Show();
    void Hide();
    void Destroy(); 
}
}