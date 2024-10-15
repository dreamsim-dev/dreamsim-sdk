using System.Collections.Generic;
using System.Linq;
using AppsFlyerSDK;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class AppsFlyerManager : MonoBehaviour, IAppsFlyerUserInvite
{
    public void Init(Settings.AnalyticsSettings.AppsFlyerSettings settings)
    {
        var devKey = settings.DevKey;
        var appId = settings.AppId;

        AppsFlyer.initSDK(devKey, appId);
        AppsFlyer.setIsDebug(settings.Debug);
        AppsFlyer.enableTCFDataCollection(true);
        // var trackingEnabled = DreamsimApp.Analytics.TrackingEnabled;
        // var consent = AppsFlyerConsent.ForGDPRUser(trackingEnabled, trackingEnabled);
        // AppsFlyer.setConsentData(consent);
        // DreamsimLogger.Log($"AppsFlyer consent set to {trackingEnabled}");
        AppsFlyer.startSDK();
        DreamsimLogger.Log("AppsFlyer initiated");
    }

    public string GetAppsFlyerId() { return AppsFlyer.getAppsFlyerId(); }
    
    internal void AttributeAndOpenStore(string appId, string campaign, List<EventParam> eventParams)
    {
        var @params = eventParams
            .ToDictionary(eventParam => eventParam.Key, eventParam => eventParam.Value.ToString());
        
        AppsFlyer.attributeAndOpenStore(appId, campaign, @params, this);
    }

    public void onInviteLinkGenerated(string link)
    {
        // Intentionally empty
    }

    public void onInviteLinkGeneratedFailure(string error)
    {
        // Intentionally empty
    }

    public void onOpenStoreLinkGenerated(string link)
    {
        DreamsimLogger.Log($"Cross promo link generated: {link}");
        #if UNITY_IOS
        Application.OpenURL(link);
        #endif
    }
}
}