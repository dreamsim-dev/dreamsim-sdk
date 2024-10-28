using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using DevToDev.Analytics;
using UnityEngine;
using UnityEngine.Serialization;

[assembly: InternalsVisibleTo("Dreamsim.Publishing.Editor")]

namespace Dreamsim.Publishing
{
 public class Settings : ScriptableObject
 {
     [Serializable]
     public class GeneralSettings
     {
         [SerializeField]
         private string _iosStoreAppId;

         [SerializeField]
         private bool _useAnalytics = true;

         [SerializeField]
         private bool _useAdvertisement = true;

         internal bool UseAnalytics => _useAnalytics;
         internal bool useAdvertisement => _useAdvertisement;

         internal string StoreAppId => (Application.isEditor
             ? string.Empty
             : Application.platform == RuntimePlatform.Android
                 ? Application.identifier
                 : _iosStoreAppId).Trim();
     }
     
     [Serializable]
     public class AdvertisementSettings
     {
         internal enum Mediators 
         {
             None = 0,
             IronSource = 1,
             AppLovin = 2,
         }
         
         [SerializeField]
         private Mediators _mediation;
         
         internal Mediators Mediation => _mediation;
         
         [Serializable]
         public class LevelPlaySettings
         {
             [SerializeField]
             private string _iosAppKey;
             
             [SerializeField]
             private string _androidAppKey;

             [SerializeField] private bool _useRewardedVideo;
             
             internal string AppKey => (Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppKey
                     : _iosAppKey).Trim();

             internal bool UseRewardedVideo => _useRewardedVideo;
         }
         
         [Serializable]
         public class AppLovinSettings
         {
             [SerializeField]
             private string _sdkKey;
             
             [SerializeField]
             private string _iosUnitId;
             
             [SerializeField]
             private string _androidUnitId;

             [SerializeField] private bool _useRewardedVideo;
             
             internal string UnitId => (Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidUnitId
                     : _iosUnitId).Trim();

             internal string SdkKey => _sdkKey.Trim();
             internal bool UseRewardedVideo => _useRewardedVideo;
         }

         [Serializable]
         public class AdMobSettings
         {
             [SerializeField]
             private string _iosAppId;

             [SerializeField]
             private string _androidAppId;

             internal string iOSAppId => _iosAppId.Trim();
             internal string AndroidAppId => _androidAppId.Trim();
         }

         [SerializeField]
         private LevelPlaySettings _levelPlay;
         
         [SerializeField]
         private AppLovinSettings _appLovin;

         [SerializeField]
         private AdMobSettings _adMob;

         internal LevelPlaySettings LevelPlay => _levelPlay;
         internal AppLovinSettings AppLovin => _appLovin;
         internal AdMobSettings AdMob => _adMob;
     }
     
     [Serializable]
     public class AnalyticsSettings
     {
         [Serializable]
         public class AppsFlyerSettings
         {
             [SerializeField]
             private string _iosDevKey;

             [SerializeField]
             private string _androidDevKey;

             [SerializeField]
             private bool _debug;

             internal string DevKey => (Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidDevKey
                     : _iosDevKey).Trim();

             internal bool Debug => _debug;
         }
         
         [Serializable]
         public class DevToDevSettings
         {
             [SerializeField]
             private string _iosAppId;

             [SerializeField]
             private string _androidAppId;

             [SerializeField]
             private DTDLogLevel _logLevel;

             internal string AppId => (Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppId
                     : _iosAppId).Trim();

             internal DTDLogLevel LogLevel => _logLevel;
         }

         [SerializeField]
         private string _purchaseValidatorSlug;

         [SerializeField]
         private AppsFlyerSettings _appsFlyer;

         [SerializeField]
         private DevToDevSettings _devToDev;

         internal string PurchaseValidatorSlug => _purchaseValidatorSlug.Trim();
         internal AppsFlyerSettings AppsFlyer => _appsFlyer;
         internal DevToDevSettings DevToDev => _devToDev;
     }

     [Serializable]
     public class FacebookSettings
     {
         [SerializeField]
         private string _appLabel;
         
         [SerializeField]
         private string _appId;

         [SerializeField]
         private string _clientToken;

         public string AppLabel => _appLabel;
         public string AppId => _appId;
         public string ClientToken => _clientToken;
     }

     [Serializable]
     public class GDPRSettings
     {
         /// <summary>
         /// How to obtain: https://developers.google.com/admob/unity/privacy#testing
         /// </summary>
         [SerializeField]
         [Tooltip("How to obtain: https://developers.google.com/admob/unity/privacy#testing")]
         private List<string> _googleMobileAdsTestDeviceHashedIds;
         
         internal List<string> GoogleMobileAdsTestDeviceHashedIds => _googleMobileAdsTestDeviceHashedIds;
     }
     
     private const string AssetFileName = "[Dreamsim] Publishing Settings";

     public static Settings Find()
     {
         return Resources.Load<Settings>(AssetFileName);
     }

     #if UNITY_EDITOR
     public static Settings Create()
     {
         var settings = CreateInstance<Settings>();
         const string relativePath = "Dreamsim/Publishing/Resources";
         var dir = Path.Combine(Application.dataPath, relativePath);
         if (!Directory.Exists(dir))
         {
             Directory.CreateDirectory(dir);
         }
         
         UnityEditor.AssetDatabase.CreateAsset(settings, $"Assets/{relativePath}/{AssetFileName}.asset");
         return settings;
     }
     #endif

     [SerializeField]
     private GeneralSettings _general;

     [SerializeField]
     private AnalyticsSettings _analytics;

     [SerializeField]
     private AdvertisementSettings _advertisement;

     [SerializeField]
     private FacebookSettings _facebook;

     [SerializeField]
     private GDPRSettings _gdpr;

     internal GeneralSettings General => _general;
     internal AnalyticsSettings Analytics => _analytics;
     internal AdvertisementSettings Advertisement => _advertisement;
     internal FacebookSettings Facebook => _facebook;
     internal GDPRSettings GDPR => _gdpr;
 }
}