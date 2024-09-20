using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
    public interface IAnalyticsLogger
    {
        void Log(string eventName);

        void Log(string eventName, List<EventParam> eventParams);
        
        void LogPurchase(PurchaseEventArgs args);

        void LogPurchaseInitiation(Product product);

        void LogRewardedAdImpression(string adSource);

        void LogRewardedAdRequest(string adSource);

        void LogRewardedAdClicked(string adSource);

        void LogRewardedAdRewardReceived(string adSource);
        
        void LogTutorialStart();

        void LogTutorialSkipped();

        void LogTutorialStepCompletion(int step);

        void LogTutorialCompletion();

        void LogLevelUp(int level);

        void LogContentView(string contentId);

        void LogNetworkReachability(bool isReachable);
    }
}