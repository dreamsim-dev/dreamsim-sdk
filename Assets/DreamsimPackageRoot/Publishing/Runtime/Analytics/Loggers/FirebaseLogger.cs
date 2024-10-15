using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
public class FirebaseLogger : IInternalAnalyticsLogger
{
    public void Log(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }

    public void Log(string eventName, List<EventParam> eventParams)
    {
        var @params = new List<Parameter>();
        foreach (var eventParam in eventParams)
        {
            if (eventParam.ValueType == typeof(long)) @params.Add(new Parameter(eventParam.Key, (long)eventParam.Value));
            if (eventParam.ValueType == typeof(double)) @params.Add(new Parameter(eventParam.Key, (double)eventParam.Value));
            if (eventParam.ValueType == typeof(string)) @params.Add(new Parameter(eventParam.Key, (string)eventParam.Value));
        }
        
        FirebaseAnalytics.LogEvent(eventName, @params.ToArray());
    }

    public void LogPurchase(PurchaseEventArgs args)
    {
        // Intentionally empty
    }

    public void LogFirstPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;
        var @params = new Parameter[]
        {
            new("order_id", product.transactionID), new("product_id", product.definition.id),
        };

        FirebaseAnalytics.LogEvent("first_purchase", @params);
    }

    public void LogPurchaseInitiation(Product product) { FirebaseAnalytics.LogEvent("initiated_checkout"); }

    public void LogRewardedAdImpression(string adSource)
    {
        // Intentionally empty
        // Logs at IronSource revenue callback
    }

    public void LogRewardedAdRequest(string adSource)
    {
        var @params = new Parameter[] { new("ad_source", adSource) };
        FirebaseAnalytics.LogEvent("ad_request", @params);
    }

    public void LogRewardedAdClicked(string adSource)
    {
        var @params = new Parameter[] { new("ad_source", adSource) };
        FirebaseAnalytics.LogEvent("ad_request", @params);
    }

    public void LogRewardedAdRewardReceived(string adSource)
    {
        var @params = new Parameter[] { new("ad_source", adSource) };
        FirebaseAnalytics.LogEvent("ad_reward", @params);
    }

    public void LogRewardedAdRewardReceivedTimes(int times)
    {
        FirebaseAnalytics.LogEvent($"ad_reward_received_{times}_times");
    }

    public void LogCrossPromoImpression(string appId, string campaign, List<EventParam> eventParams)
    {
        // Intentionally empty
    }

    public void LogTutorialStart()
    {
        FirebaseAnalytics.LogEvent("tutorial_start");
    }

    public void LogTutorialSkipped()
    {
        FirebaseAnalytics.LogEvent("tutorial_skipped");
    }

    public void LogTutorialStepCompletion(int step)
    {
        var @params = new Parameter[] { new ("step", step) };
        FirebaseAnalytics.LogEvent($"tutorial_step_completion", @params);
    }

    public void LogTutorialCompletion()
    {
        FirebaseAnalytics.LogEvent($"tutorial_completed");
    }

    public void LogLevelUp(int level)
    {
        var @params = new Parameter[] { new("level", level.ToString()) };
        FirebaseAnalytics.LogEvent("level_up", @params);
    }

    public void LogContentView(string contentId)
    {
        var @params = new Parameter[] { new("content_id", contentId) };
        FirebaseAnalytics.LogEvent("content_view", @params);
    }

    public void LogNetworkReachability(bool isReachable)
    {
        var @params = new Parameter[] { new("is_reachable", isReachable.ToString()) };
        FirebaseAnalytics.LogEvent("network_reachability", @params);
    }
}
}