using DevToDev.Analytics;
using DevToDev.Messaging;
using DevToDev.Messaging.Platform;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class DevToDevManager : MonoBehaviour
{
    private class PushListener : IDTDPushListener
    {
        public void OnPushServiceRegistrationSuccessful(string deviceId) { DreamsimLogger.Log(deviceId); }

        public void OnPushServiceRegistrationFailed(string error) { DreamsimLogger.Log(error); }

        public void OnPushNotificationReceived(DTDPushMessage message) { DreamsimLogger.Log(message); }

        public void OnInvisibleNotificationReceived(DTDPushMessage message) { DreamsimLogger.Log(message); }

        public void OnPushNotificationOpened(DTDPushMessage pushMessage, DTDActionButton actionButton)
        {
            DreamsimLogger.Log(pushMessage.ToString());
            DreamsimLogger.Log(actionButton.ToString());
        }
    }

    public void Init(Settings.AnalyticsSettings.DevToDevSettings settings, string appsFlyerId)
    {
        var appId = settings.AppId;
        var config = new DTDAnalyticsConfiguration
        {
            TrackingAvailability = DTDTrackingStatus.Enable,
            LogLevel = settings.LogLevel
        };

        DTDUserCard.Set("ad_tracker_id", appsFlyerId);
        DTDAnalytics.Initialize(appId, config);
        DreamsimLogger.Log("DevToDev initialized");

        #if UNITY_ANDROID
        DTDMessaging.Android.SetPushListener(new PushListener());
        DTDMessaging.Android.Initialize();
        DTDMessaging.Android.StartPushService();
        #elif UNITY_IOS
        DTDMessaging.IOS.SetNotificationOptions(DTDNotificationOptions.Alert
            | DTDNotificationOptions.Badge
            | DTDNotificationOptions.Sound);
        DTDMessaging.IOS.SetPushListener(new PushListener());
        DTDMessaging.IOS.StartNotificationService();
        #endif
        DreamsimLogger.Log("DevToDev push service started");
    }
}
}