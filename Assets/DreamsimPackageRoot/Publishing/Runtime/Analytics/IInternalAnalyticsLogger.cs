using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
    internal interface IInternalAnalyticsLogger : IAnalyticsLogger
    {
        void LogFirstPurchase(PurchaseEventArgs args);
        
        void LogRewardedAdRewardReceivedTimes(int times);

        void LogCrossPromoImpression(string appId, string campaign, List<EventParam> @params);
    }
}