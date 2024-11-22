using System;
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

        
        internal void Init(IMediationBridge mediator)
        {
            _mediation = mediator;

           _mediation.SubscribeBannerAdLoaded(adSource => OnAdLoaded?.Invoke(adSource));
            _mediation.SubscribeBannerAdLoadFailed(adSource => OnAdLoadFailed?.Invoke(adSource));
            _mediation.SubscribeBannerAdDisplayed(adSource => OnAdDisplayed?.Invoke(adSource));
            _mediation.SubscribeBannerAdDisplayFailed(adSource => OnAdDisplayFailed?.Invoke(adSource));
            
            _mediation.SubscribeBannerAdClicked(adSource => OnAdClicked?.Invoke(adSource));
            _mediation.SubscribeBannerAdCollapsed(adSource => OnAdCollapsed?.Invoke(adSource));
            _mediation.SubscribeBannerAdLeftApplication(adSource=> OnAdLeftApplication.Invoke(adSource));
            _mediation.SubscribeBannerAdExpanded(adSource => OnAdExpanded?.Invoke(adSource));
           
            DreamsimLogger.Log("Banner ads initialized");
        }

        
        public void LoadBanner()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            //_mediation.LoadBanner();
            DreamsimLogger.Log("Banner loading requested");
        }

        public void ShowBanner()
        {
            IronSource.Agent.displayBanner();
            //_mediation.ShowBanner();
            DreamsimLogger.Log($"Banner  is shown.");
        }

        public void HideBanner()
        {
            IronSource.Agent.hideBanner();
            //_mediation.HideBanner();
            DreamsimLogger.Log("Banner hidden.");
        }

        public void DestroyBanner()
        {
            IronSource.Agent.destroyBanner();
          //_mediation.DestroyBanner();
            DreamsimLogger.Log("Banner destroyed.");
        }
    }
}
