using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
    internal interface IInternalAnalyticsLogger : IAnalyticsLogger
    {
        void LogFirstPurchase(PurchaseEventArgs args);
        
        void LogRewardedAdRewardReceivedTimes(int times);
    }
}