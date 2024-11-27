using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppsFlyerSDK;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
public class AppsFlyerLogger : IInternalAnalyticsLogger
{
    public void Log(string eventName) { AppsFlyer.sendEvent(eventName, null); }

    public void Log(string eventName, List<EventParam> eventParams)
    {
        var @params = eventParams
            .ToDictionary(eventParam => eventParam.Key, eventParam => eventParam.Value.ToString());

        AppsFlyer.sendEvent(eventName, @params);
    }

    public void LogPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;
        var @params = new Dictionary<string, string>
        {
            { AFInAppEvents.REVENUE, product.metadata.localizedPrice.ToString(CultureInfo.InvariantCulture) },
            { AFInAppEvents.CURRENCY, product.metadata.isoCurrencyCode },
            { AFInAppEvents.QUANTITY, 1.ToString() },
            { AFInAppEvents.CONTENT_ID, product.definition.id },
            { "af_order_id", product.transactionID },
        };

        AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, @params);
    }

    public void LogFirstPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;
        var @params = new Dictionary<string, string>
        {
            { AFInAppEvents.CONTENT_ID, product.definition.id },
            { "af_order_id", product.transactionID },
        };

        AppsFlyer.sendEvent("first_purchase", @params);
    }

    public void LogPurchaseInitiation(Product product) { AppsFlyer.sendEvent(AFInAppEvents.INITIATED_CHECKOUT, null); }

    public void LogRewardedAdImpression(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_rewarded_impression", @params);
    }

    public void LogRewardedAdRequest(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_rewarded_request", @params);
    }

    public void LogRewardedAdClicked(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_rewarded_clicked", @params);
    }

    public void LogRewardedAdRewardReceived(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_rewarded_reward", @params);
    }

    public void LogBannerAdRequest(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_banner_request", @params);
    }

    public void LogBannerAdImpression(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_banner_impression", @params);
    }

    public void LogBannerAdClicked(string adSource)
    {
        var @params = new Dictionary<string, string> { { "ad_source", adSource } };
        AppsFlyer.sendEvent("ad_banner_clicked", @params);
    }

    public void LogRewardedAdRewardReceivedTimes(int times) { AppsFlyer.sendEvent(AFInAppEvents.ADD_TO_CART, null); }

    public void LogCrossPromoImpression(string appId, string campaign, List<EventParam> eventParams)
    {
        var @params = eventParams
            .ToDictionary(eventParam => eventParam.Key, eventParam => eventParam.Value.ToString());

        AppsFlyer.recordCrossPromoteImpression(appId, campaign, @params);
    }

    public void LogTutorialStart() { AppsFlyer.sendEvent("tutorial_start", null); }

    public void LogTutorialSkipped() { AppsFlyer.sendEvent("tutorial_skipped", null); }

    public void LogTutorialStepCompletion(int step)
    {
        var @params = new Dictionary<string, string> { { "step", step.ToString() } };
        AppsFlyer.sendEvent("tutorial_step_completion", @params);
    }

    public void LogTutorialCompletion() { AppsFlyer.sendEvent(AFInAppEvents.TUTORIAL_COMPLETION, null); }

    public void LogLevelUp(int level)
    {
        var @params = new Dictionary<string, string> { { AFInAppEvents.LEVEL, level.ToString() } };
        AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, @params);
    }

    public void LogContentView(string contentId)
    {
        var @params = new Dictionary<string, string> { { "content_id", contentId } };
        AppsFlyer.sendEvent("content_view", @params);
    }

    public void LogNetworkReachability(bool isReachable)
    {
        var @params = new Dictionary<string, string> { { "is_reachable", isReachable.ToString() } };
        AppsFlyer.sendEvent("network_reachability", @params);
    }
}
}