# Dreamsim SDK
UPM package for publishing purposes.
## Features
- Analytics
    - AppsFlyer
    - devtodev
    - Firebase
    - Facebook
- Network reachability tracking (analytics event)
- Advertisement
    - IronSource/LevelPlay
- ATT (App Tracking Transparency) flow
- DMA (Google AdMob consent) flow
- In-app purchases server-based fraud filter
## Installation
1. Add OpenUPM as a scoped registry using [documentation](https://developers.google.com/admob/unity/quick-start#import_the_mobile_ads_for_unity_plugin) or simply add following entry into manifest.json (at root level):
   ```json
   "scopedRegistries": [
     {
       "name": "OpenUPM",
       "url": "https://package.openupm.com",
       "scopes": [
         "com.google"
       ]
     }
   ]
   ```
2. Install following dependencies via PackageManager:
    - GoogleMobileAds SDK ([GitHub](https://github.com/googleads/googleads-mobile-unity))
    - AppsFlyer SDK ([GitHub](https://github.com/AppsFlyerSDK/appsflyer-unity-plugin), [Documentation](https://dev.appsflyer.com/hc/docs/installation))
    - devtodev SDK Analytics + Messaging ([GitHub](https://github.com/devtodev-analytics/Unity-sdk-3.0), [Documentation](https://docs.devtodev.com/integration/integration-of-sdk-v2/sdk-integration/unity))
    - UniTask ([GitHub](https://github.com/Cysharp/UniTask))
    - DeviceHelper ([GitHub](https://github.com/lexscite/UnityDeviceHelper))
    - IngameDebugConsole ([GitHub](https://github.com/yasirkula/UnityIngameDebugConsole))
3. Manually install following dependencies (via .unitypackage):
    - Facebook SDK ([GitHub](https://github.com/facebook/facebook-sdk-for-unity))
    - IronSource/LevelPlay SDK ([Documentation](https://developers.is.com/ironsource-mobile/unity/unity-plugin))
    - Firebase SDK (Analytics) ([GItHub](https://github.com/firebase/firebase-unity-sdk))
4. Install upm branch of this repository via PackageManager.
   ```
   https://github.com/dreamsim-dev/dreamsim-sdk.git#upm
   ```
### Integration
1. Fill FacebookSDKSettings.asset.
2. Fill Dreamsim Publishing Settings (Toolbar -> Dreamsim -> Publishing Settings).
3. Press "Update Dependencies" button in Dreamsim Publishing Settings.
4. Add following sting to Android.manifest under "manifest" section:
   ```
   <uses-permission android:name="com.google.android.gms.permission.AD_ID" />
   ```
5. Add following sting to Android.manifest under "application" section:
   ```
   <meta-data android:name="com.google.android.gms.ads.APPLICATION_ID" android:value="ca-app-pub-5596173413050708~1787262028"/>
   ```
### Additional Dependencies
- Unity Purchasing
- JsonDotNet (?)
## Initialization
Add following code somewhere at your composition root. Before any analytics event may happen.
```cs
// Creating basic SDK GameObject.
DreamsimCommon.Create();
// Create publishing SDK GameObject.
DreamsimPublishing.Create();

// Initializing publishing part of the SDK.
// Await this method before any analytics event may happen.
await DreamsimPublishing.InitAsync();
```
## Advertisement
### Serving
```cs
// Use following method to show ad. Use different placements for each ad point in application.
DreamsimPublishing.Advertisement.RewardedVideo.Show(string placement)

// Check app availability via following method.
DreamsimPublishing.Advertisement.RewardedVideo.IsAvailable
```
### Callbacks
```cs
// Ad completed (give reward here).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdCompleted

// Ad closed (application took control).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdClosed;

/// Loading error.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdLoadFailed;

// Impression error.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdShowFailed;

// Availability changed.
DreamsimPublishing.Advertisement.RewardedVideo.OnAvailabilityChanged;

// Ad opened (impression).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdOpened;

// Ad clicked. Ad itself. Not button.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdClicked;
```
## Analytics
### Custom Events
Use only if no basic alternative presents.
```cs
DreamsimPublishing.Analytics.Log(string eventName)

DreamsimPublishing.Analytics.Log(string eventName, List<EventParam> eventParams)
```
### Basic Events
```cs
// In-app purchase.
DreamsimPublishing.Analytics.LogPurchase(PurchaseEventArgs args)

// Purchase initiation (e.g. purchase button click).
DreamsimPublishing.Analytics.LogPurchaseInitiation(Product product)

// Rewarded video ad request (e.g. ad button click).
DreamsimPublishing.Analytics.LogRewardedAdRequest(string adSource)

// Click on rewarded video ad.
DreamsimPublishing.Analytics.LogRewardedAdClicked(string adSource)

// Reward received.
DreamsimPublishing.Analytics.LogRewardedAdRewardReceived(string adSource)

// Tutorial start.
DreamsimPublishing.Analytics.LogTutorialStart()

// Tutorial skip (explicit opt-out).
DreamsimPublishing.Analytics.LogTutorialSkipped()

// Tutorial step completion (starts from 1).
DreamsimPublishing.Analytics.LogTutorialStepCompletion(int step)

// Tutorial completion.
DreamsimPublishing.Analytics.LogTutorialCompletion()

// LevelUP (e.g. new work applied in case of "sim" game).
DreamsimPublishing.Analytics.LogLevelUp(int level)

// Content view (contentId is unique string). Prefer using enum with ToString() method.
DreamsimPublishing.Analytics.LogContentView(string contentId)
```
### Automatically Logging Events
```cs
DreamsimPublishing.Analytics.LogFirstPurchase(PurchaseEventArgs args)

DreamsimPublishing.Analytics.LogRewardedAdRewardReceivedTimes(int times)
```