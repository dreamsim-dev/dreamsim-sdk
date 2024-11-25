using System;
using com.unity3d.mediation;
using UnityEngine;

namespace Dreamsim.Publishing
{
    public class BannerAdListener
    {
        public event Action<string> OnAdLoaded;
        public event Action<string> OnAdLoadFailed;
        public event Action<string> OnAdClicked;
        public event Action<string> OnAdDisplayed;
        public event Action<string> OnAdDisplayFailed;
        public event Action<string> OnAdCollapsed;
        public event Action<string> OnAdLeftApplication;
        public event Action<string> OnAdExpanded;
        
        private IMediationBridge _mediation;

        public enum BannerSize
        {
            Banner,             // Standard banner	320 x 50
            Large,              // Large banner	320 x 90
            MediumRectangle,    // Medium Rectangular (MREC)	300 x 250
            Adaptive            // Automatically renders ads to adjust size and orientation for mobile & tablets. Device width X recommended height
        }

        public enum BannerPosition
        {
            Top,
            Bottom
        }
        
        internal void Init(IMediationBridge mediator)
        {
            _mediation = mediator;

           DreamsimLogger.Log("Banner ads initialized");
        }

        
        public void Load(BannerSize size = BannerSize.Banner, BannerPosition position = BannerPosition.Bottom)
        {
            #if DREAMSIM_USE_IRONSOURCE
            if (size == BannerSize.Adaptive)
            {
                var adaptiveSize = LevelPlayAdSize.CreateAdaptiveAdSize();  
                IronSource.Agent.loadBanner(adaptiveSize, ConvertPosition(position)); 
            }
            else
            {
                var ironSize = ConvertSize(size);  
                var ironPosition = ConvertPosition(position);
                IronSource.Agent.loadBanner(ironSize, ironPosition);
            }
            DreamsimLogger.Log("Banner loading requested");
            #endif
            //_mediation.LoadBanner();
            
        }
        public void Show()
        {
            #if DREAMSIM_USE_IRONSOURCE
            IronSource.Agent.displayBanner();
            DreamsimLogger.Log($"Banner is shown.");
            #endif
        }
        public void Hide()
        {
            #if DREAMSIM_USE_IRONSOURCE
            IronSource.Agent.hideBanner();
            DreamsimLogger.Log("Banner hidden.");
            #endif
            //_mediation.HideBanner();
        }
        public void Destroy()
        {
            #if DREAMSIM_USE_IRONSOURCE
            IronSource.Agent.destroyBanner();
            DreamsimLogger.Log("Banner destroyed.");
            #endif
            //_mediation.DestroyBanner();
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
    }
}

