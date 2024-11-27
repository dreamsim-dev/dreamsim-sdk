using System.Collections.Generic;
using DevToDev.Analytics;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
public class DevToDevLogger : IInternalAnalyticsLogger
{
    public void Log(string eventName) { DTDAnalytics.CustomEvent(eventName); }

    public void Log(string eventName, List<EventParam> eventParams)
    {
        var @params = new DTDCustomEventParameters();
        foreach (var eventParam in eventParams)
        {
            if (eventParam.ValueType == typeof(long)) @params.Add(eventParam.Key, (long)eventParam.Value);
            if (eventParam.ValueType == typeof(double)) @params.Add(eventParam.Key, (double)eventParam.Value);
            if (eventParam.ValueType == typeof(string)) @params.Add(eventParam.Key, (string)eventParam.Value);
        }

        DTDAnalytics.CustomEvent(eventName, @params);
    }

    public void LogPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;
        DTDAnalytics.RealCurrencyPayment(product.transactionID,
            (float)product.metadata.localizedPrice,
            product.definition.id,
            product.metadata.isoCurrencyCode);
    }

    public void LogFirstPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;
        var @params = new DTDCustomEventParameters();
        @params.Add("order_id", product.transactionID);
        @params.Add("product_id", product.definition.id);
        DTDAnalytics.CustomEvent("first_purchase", @params);
    }

    public void LogPurchaseInitiation(Product product) { DTDAnalytics.CustomEvent("initiated_checkout"); }

    public void LogRewardedAdImpression(string adSource)
    {
        // Intentionally empty
        // Logs at IronSource revenue callback
    }

    public void LogRewardedAdRequest(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_rewarded_request", @params);
    }

    public void LogRewardedAdClicked(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_rewarded_clicked", @params);
    }

    public void LogRewardedAdRewardReceived(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_rewarded_reward", @params);
    }

    public void LogBannerAdRequest(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_banner_request", @params);
    }

    public void LogBannerAdImpression(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_banner_impression", @params);
    }

    public void LogBannerAdClicked(string adSource)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("ad_source", adSource);
        DTDAnalytics.CustomEvent("ad_banner_clicked", @params);
    }

    public void LogRewardedAdRewardReceivedTimes(int times)
    {
        DTDAnalytics.CustomEvent($"ad_rewarded_reward_received_{times}_times");
    }

    public void LogCrossPromoImpression(string appId, string campaign, List<EventParam> eventParams)
    {
        // Intentionally empty
    }

    public void LogTutorialStart() { DTDAnalytics.Tutorial(-1); }

    public void LogTutorialSkipped() { DTDAnalytics.Tutorial(0); }

    public void LogTutorialStepCompletion(int step)
    {
        if (step < 1) DreamsimLogger.LogError($"DevToDev. Tutorial steps should start from 1 ({step})");
        DTDAnalytics.Tutorial(step);
    }

    public void LogTutorialCompletion() { DTDAnalytics.Tutorial(-2); }

    public void LogLevelUp(int level) { DTDAnalytics.LevelUp(level); }

    public void LogContentView(string contentId)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("content_id", contentId);
        DTDAnalytics.CustomEvent("content_view", @params);
    }

    public void LogNetworkReachability(bool isReachable)
    {
        var @params = new DTDCustomEventParameters();
        @params.Add("is_reachable", isReachable);
        DTDAnalytics.CustomEvent("network_reachability", @params);
    }
}
}