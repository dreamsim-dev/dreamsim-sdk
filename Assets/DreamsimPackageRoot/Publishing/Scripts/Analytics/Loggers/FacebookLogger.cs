using System.Collections.Generic;
using UnityEngine.Purchasing;
using Product = UnityEngine.Purchasing.Product;

namespace Dreamsim.Publishing
{
public class FacebookLogger : IInternalAnalyticsLogger
{
    public void Log(string eventName)
    {
        // Intentionally empty
    }

    public void Log(string eventName, List<EventParam> eventParams)
    {
        // Intentionally empty
    }

    public void LogPurchase(PurchaseEventArgs args)
    {
        // Intentionally empty
    }

    public void LogFirstPurchase(PurchaseEventArgs args)
    {
        // Intentionally empty
    }

    public void LogPurchaseInitiation(Product product)
    {
        // Intentionally empty
    }

    public void LogRewardedAdImpression(string adSource)
    {
        // Intentionally empty
    }

    public void LogRewardedAdRequest(string adSource)
    {
        // Intentionally empty
    }

    public void LogRewardedAdClicked(string adSource)
    {
        // Intentionally empty
    }

    public void LogRewardedAdRewardReceived(string adSource)
    {
        // Intentionally empty
    }

    public void LogTutorialStart()
    {
        // Intentionally empty
    }

    public void LogTutorialSkipped()
    {
        // Intentionally empty
    }

    public void LogRewardedAdRewardReceivedTimes(int times)
    {
        // Intentionally empty
    }

    public void LogTutorialStepCompletion(int step)
    {
        // Intentionally empty
    }

    public void LogTutorialCompletion()
    {
        // Intentionally empty
    }

    public void LogLevelUp(int level)
    {
        // Intentionally empty
    }

    public void LogContentView(string contentId)
    {
        // Intentionally empty
    }

    public void LogNetworkReachability(bool isReachable)
    {
        // Intentionally empty
    }
}
}