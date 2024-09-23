using AppsFlyerSDK;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class AppsFlyerManager : MonoBehaviour
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
}
}