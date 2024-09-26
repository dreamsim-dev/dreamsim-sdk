# Dreamsim SDK
UPM package for publishing purposes.
## Installation
1. Add OpenUPM as a scoped registry using [documentation](https://developers.google.com/admob/unity/quick-start#import_the_mobile_ads_for_unity_plugin) or simply add following entry into manifest.json:
   ```
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
   - AppsFlyer SDK ([GitHub](https://github.com/AppsFlyerSDK/appsflyer-unity-plugin), [Documentation](https://dev.appsflyer.com/hc/docs/installation))
   - devtodev SDK Analytics + Messaging ([GitHub](https://github.com/devtodev-analytics/Unity-sdk-3.0), [Documentation](https://docs.devtodev.com/integration/integration-of-sdk-v2/sdk-integration/unity))
   - UniTask ([GitHub](https://github.com/Cysharp/UniTask))
   - DeviceHelper ([GitHub](https://github.com/lexscite/UnityDeviceHelper))
   - IngameDebugConsole ([GitHub](https://github.com/yasirkula/UnityIngameDebugConsole))
3. Manually install following dependencies (via .unitypackage)
   - Facebook SDK ([GitHub](https://github.com/facebook/facebook-sdk-for-unity))
   - GoogleMobileAds SDK ([GitHub](https://github.com/googleads/googleads-mobile-unity))
   - IronSource/LevelPlay SDK ([Documentation](https://developers.is.com/ironsource-mobile/unity/unity-plugin))
   - Firebase SDK (Analytics) ([GItHub](https://github.com/firebase/firebase-unity-sdk))
4. Install upm branch of this repository via PackageManager
   ```
   https://github.com/dreamsim-dev/dreamsim-sdk.git#upm
   ```
5. Fill in Dreamsim Publishing Settings (Toolbar -> Dreamsim -> Publishing Settings)
## Additional Dependencies
- Unity Purchasing
- JsonDotNet (?)
## Initialization
Add following code somewhere at your composition root. Before any analytics event may happen.
```
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
Use following method to show ad. Use different placements for each ad point in application.
```
DreamsimPublishing.Advertisement.RewardedVideo.Show(string placement)
```
Check app availability via following method.
```
DreamsimPublishing.Advertisement.RewardedVideo.IsAvailable
```
### Callbacks
```
// Ad completed (give reward here).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdCompleted

// Ad closed (application took control).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdClosed

/// Loading error.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdLoadFailed

// Impression error.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdShowFailed

// Availability changed.
DreamsimPublishing.Advertisement.RewardedVideo.OnAvailabilityChanged

// Ad opened (impression).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdOpened

// Ad clicked. Ad itself. Not button.
DreamsimPublishing.Advertisement.RewardedVideo.OnAdClicked
```
## Analytics
### Custom events
Use only if no basic alternative presents.
```
DreamsimPublishing.Analytics.Log(string eventName)
```
```
DreamsimPublishing.Analytics.Log(string eventName, List<EventParam> eventParams)
```
### Basic events

In-app purchase.
```
DreamsimPublishing.Analytics.LogPurchase(PurchaseEventArgs args)
```
Purchase initiation (e.g. purchase button click).
```
DreamsimPublishing.Analytics.LogPurchaseInitiation(Product product)
```
Rewarded video ad request (e.g. ad button click).
```
DreamsimPublishing.Analytics.LogRewardedAdRequest(string adSource)
```
Click on rewarded video ad.
```
DreamsimPublishing.Analytics.LogRewardedAdClicked(string adSource)
```
Reward received.
```
DreamsimPublishing.Analytics.LogRewardedAdRewardReceived(string adSource)
```
Tutorial start.
```
DreamsimPublishing.Analytics.LogTutorialStart()
```
Tutorial skip (explicit opt-out).
```
DreamsimPublishing.Analytics.LogTutorialSkipped()
```
Tutorial step completion (starts from 1).
```
DreamsimPublishing.Analytics.LogTutorialStepCompletion(int step)
```
Tutorial completion.
```
DreamsimPublishing.Analytics.LogTutorialCompletion()
```
LevelUP (e.g. new work applied in case of "sim" game).
```
DreamsimPublishing.Analytics.LogLevelUp(int level)
```
Content view (contentId is unique string). Prefer using enum with ToString() method.
```
DreamsimPublishing.Analytics.LogContentView(string contentId)
```
### Automatically logging events
```
DreamsimPublishing.Analytics.LogFirstPurchase(PurchaseEventArgs args)
```
```
DreamsimPublishing.Analytics.LogRewardedAdRewardReceivedTimes(int times)
```