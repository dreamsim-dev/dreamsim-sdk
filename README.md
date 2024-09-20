# Dreamsim SDK
UPM package for publishing purposes.
## Installation
1. Add OpenUPM as a scoped registry ([documentation](https://developers.google.com/admob/unity/quick-start#import_the_mobile_ads_for_unity_plugin))
2. Manually install following dependencies (prefer installation via PackageManager):
   - AppsFlyer SDK (https://github.com/AppsFlyerSDK/appsflyer-unity-plugin)
   - devtodev SDK Analytics + Messaging (https://github.com/devtodev-analytics/Unity-sdk-3.0)
   - Facebook SDK (https://github.com/facebook/facebook-sdk-for-unity)
   - Firebase SDK (https://github.com/firebase/firebase-unity-sdk)
   - GoogleMobileAds SDK (https://github.com/googleads/googleads-mobile-unity)
   - IronSource/LevelPlay SDK (https://developers.is.com/ironsource-mobile/unity/unity-plugin)
   - JsonDotNet
   - UniTask (https://github.com/Cysharp/UniTask)
   - DeviceHelper (https://github.com/lexscite/UnityDeviceHelper)
3. Add this repository via PackageManager
   ```
   https://github.com/dreamsim-dev/dreamsim-sdk.git#upm
   ```
4. Fill in Dreamsim settings (Toolbar -> Dreamsim -> Settings)
## Getting Started
Use DreamsimApp.OnInitialized event in order to initialize systems after Dreamsim SDK initialization.
## Additional Dependencies
- Unity Purchasing