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