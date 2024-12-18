# Dreamsim SDK
UPM package for publishing purposes.
## Features
- Analytics
    - AppsFlyer
    - devtodev
    - Firebase (currently disabled)
    - Facebook
- Advertisement
    - LevelPlay (IronSource)
      - Rewarded video
      - Banner
    - ApplovinMAX (WIP)
- Consent flow
  - ATT (App Tracking Transparency) flow
  - DMA (Google AdMob consent) flow
- In-app purchases server-based fraud filter
- Network reachability logging
- Cross-promo attribution
- CI
    - Build number available at runtime via file
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
2. Install following dependencies via PackageManager (do not manually install ExternalDependenciesManager as it comes with GoogleMobileAds):
    - UnityPurchasing
    - GoogleMobileAds SDK ([GitHub](https://github.com/googleads/googleads-mobile-unity))
    - AppsFlyer SDK ([GitHub](https://github.com/AppsFlyerSDK/appsflyer-unity-plugin), [Documentation](https://dev.appsflyer.com/hc/docs/installation))
    - devtodev SDK Analytics + Messaging ([GitHub](https://github.com/devtodev-analytics/Unity-sdk-3.0), [Documentation](https://docs.devtodev.com/integration/integration-of-sdk-v2/sdk-integration/unity))
    - UniTask ([GitHub](https://github.com/Cysharp/UniTask))
    - DeviceHelper ([GitHub](https://github.com/lexscite/UnityDeviceHelper))
    - IngameDebugConsole ([GitHub](https://github.com/yasirkula/UnityIngameDebugConsole))
3. Manually install following dependencies (via .unitypackage):
    - Facebook SDK ([GitHub](https://github.com/facebook/facebook-sdk-for-unity))
      - In case of build problems add addToAllTargets="true" to FacebookSDK/Plugins/Editor/Dependencies.xml
        ```xml
        <dependencies>
          <androidPackages>
              <androidPackage spec="com.parse.bolts:bolts-android:1.4.0" />
              <androidPackage spec="com.facebook.android:facebook-core:[17.0.0,18)" />
              <androidPackage spec="com.facebook.android:facebook-applinks:[17.0.0,18)" />
              <androidPackage spec="com.facebook.android:facebook-login:[17.0.0,18)" />
              <androidPackage spec="com.facebook.android:facebook-share:[17.0.0,18)" />
              <androidPackage spec="com.facebook.android:facebook-gamingservices:[17.0.0,18)" />
          </androidPackages>
          <iosPods>
              <iosPod name="FBSDKCoreKit_Basics" version="~> 17.0.1" addToAllTargets="true" />
              <iosPod name="FBSDKCoreKit" version="~> 17.0.1" addToAllTargets="true" />
              <iosPod name="FBSDKLoginKit" version="~> 17.0.1" addToAllTargets="true" />
              <iosPod name="FBSDKShareKit" version="~> 17.0.1" addToAllTargets="true" />
              <iosPod name="FBSDKGamingServicesKit" version="~> 17.0.1" addToAllTargets="true" />
          </iosPods>
        </dependencies>
        ```
   - Firebase SDK (Analytics) ([GItHub](https://github.com/firebase/firebase-unity-sdk))
4. Install one of following mediation plugins:
    - IronSource/LevelPlay SDK ([Documentation](https://developers.is.com/ironsource-mobile/unity/unity-plugin))
      - Don't forget to add EmbedInMobiSDK.cs and IronSourceAdQualityDependencies.xml (also described in documentation)
    - ApplovinMAX
5. Install upm branch of this repository via PackageManager (replace [VERSION] with actual value).
   ```
   https://github.com/dreamsim-dev/dreamsim-sdk.git#[VERSION]
   ```
## Integration
1. Fill Dreamsim Publishing Settings (Toolbar -> Dreamsim -> Publishing Settings) (not via ScriptableObject).
2. Select mediator.
3. Fill mediator settings.
4. Press "Update Dependencies" button in Dreamsim Publishing Settings.
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
### Rewarded Video
#### Serving
Make sure that "Use Rewarded Video" is checked in Dreamsim Publishing Settings.
```cs
// Use following method to show ad. Use different placements for each ad point in application.
DreamsimPublishing.Advertisement.RewardedVideo.Show(string placement)

// Check ad availability via following property.
DreamsimPublishing.Advertisement.RewardedVideo.IsAvailable;
```
#### Callbacks
```cs
// Ad completed (give reward here).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdCompleted;

// Ad closed (application took control).
DreamsimPublishing.Advertisement.RewardedVideo.OnAdClosed;

// Loading error.
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
### Banner
Make sure that "Use Banner" is checked in Dreamsim Publishing Settings.
#### Serving
```cs
// Use following method to load ad
DreamsimPublishing.Advertisement.Banner.Load(string adSource,
    BannerSize size = BannerSize.Default,
    BannerPosition position = BannerPosition.Bottom);

// Use following method to show ad
DreamsimPublishing.Advertisement.Banner.Show();

// Use following method to hide ad (if you will to show it again later)
DreamsimPublishing.Advertisement.Banner.Hide();

// Use following method to destroy ad
DreamsimPublishing.Advertisement.Banner.Destroy();
```
#### Callbacks
```cs
// Ad loaded
DreamsimPublishing.Advertisement.Banner.OnAdLoaded

// Ad failed to load
DreamsimPublishing.Advertisement.Banner.OnAdLoadFailed;

// Ad shown (impression)
DreamsimPublishing.Advertisement.Banner.OnAdDisplayed;

// Ad clicked
DreamsimPublishing.Advertisement.Banner.OnAdClicked;

// Ad dismissed
DreamsimPublishing.Advertisement.Banner.OnAdDismissed;

// Ad left application
DreamsimPublishing.Advertisement.Banner.OnAdLeftApplication;
```
## Analytics
### Custom Events
Use only if no basic alternative presents.
```cs
DreamsimPublishing.Analytics.Log(string eventName);

DreamsimPublishing.Analytics.Log(string eventName, List<EventParam> eventParams);
```
### Basic Events
```cs
// In-app purchase.
DreamsimPublishing.Analytics.LogPurchase(PurchaseEventArgs args);

// Purchase initiation (e.g. purchase button click).
DreamsimPublishing.Analytics.LogPurchaseInitiation(Product product);

// Tutorial start.
DreamsimPublishing.Analytics.LogTutorialStart();

// Tutorial skip (explicit opt-out).
DreamsimPublishing.Analytics.LogTutorialSkipped();

// Tutorial step completion (starts from 1).
DreamsimPublishing.Analytics.LogTutorialStepCompletion(int step);

// Tutorial completion.
DreamsimPublishing.Analytics.LogTutorialCompletion();

// LevelUP (e.g. new work applied in case of "sim" game).
DreamsimPublishing.Analytics.LogLevelUp(int level);

// Content view (contentId is unique string). Prefer using enum with ToString() method.
DreamsimPublishing.Analytics.LogContentView(string contentId);

// Cross promo impression/
DreamsimPublishing.CrossPromo.LogCrossPromoImpression(string appId, string campaign, List<EventParam> eventParams);
```
### Automatically Logging Events
```cs
// Rewarded video ad request (e.g. ad button click).
DreamsimPublishing.Analytics.LogRewardedAdRequest(string adSource);

// Rewarded video ad impression.
DreamsimPublishing.Analytics.LogRewardedAdImpression(string adSource);

// Click on rewarded video ad.
DreamsimPublishing.Analytics.LogRewardedAdClicked(string adSource);

// Rewarded video reward received.
DreamsimPublishing.Analytics.LogRewardedAdRewardReceived(string adSource);

// Banner ad request (loading started).
DreamsimPublishing.Analytics.LogBannerAdRequest(string adSource);

// Banner ad impression.
DreamsimPublishing.Analytics.LogBannerAdImpression(string adSource);

// Click on banner ad.
DreamsimPublishing.Analytics.LogBannerAdClicked(string adSource);

// First purchase.
DreamsimPublishing.Analytics.LogFirstPurchase(PurchaseEventArgs args);

// Rewrad received 30 (by default) times.
DreamsimPublishing.Analytics.LogRewardedAdRewardReceivedTimes(int times);
```
## Cross Promo
Don't forget to log cross promo impression (see [Analytics Basic Events](#basic-events) section). For attribution use following method instead of simple Application.OpenURL:
```cs
// Automatically generates attribution link and opens provided app's store page
DreamsimPublishing.CrossPromo.AttributeAndOpenStore(string appId, string campaign, List<EventParam> eventParams);
```