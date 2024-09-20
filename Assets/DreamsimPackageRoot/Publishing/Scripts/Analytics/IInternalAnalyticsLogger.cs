using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
    public interface IInternalAnalyticsLogger : IAnalyticsLogger
    {
        void LogFirstPurchase(PurchaseEventArgs args);
        
        void LogRewardedAdRewardReceivedTimes(int times);
    }
}