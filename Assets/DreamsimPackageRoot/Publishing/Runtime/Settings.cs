using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Analytics;
using UnityEngine;
[assembly: InternalsVisibleTo("Dreamsim.Publishing.Editor")]

namespace Dreamsim.Publishing
{
 public class Settings : ScriptableObject
 {
     [Serializable]
     public class AdvertisementSettings
     {
         [Serializable]
         public class LevelPlaySettings
         {
             [SerializeField]
             private string _iosAppKey;
             
             [SerializeField]
             private string _androidAppKey;

             [SerializeField] private bool _useRewardedVideo;
             
             internal string AppKey => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppKey
                     : _iosAppKey;

             internal bool UseRewardedVideo => _useRewardedVideo;
         }

         [Serializable]
         public class AdMobSettings
         {
             [SerializeField]
             private string _iosAppId;

             [SerializeField]
             private string _androidAppId;

             internal string iOSAppId => _iosAppId;
             internal string AndroidAppId => _androidAppId;
         }

         [SerializeField]
         private LevelPlaySettings _levelPlay;

         [SerializeField]
         private AdMobSettings _adMob;

         internal LevelPlaySettings LevelPlay => _levelPlay;
         internal AdMobSettings AdMob => _adMob;
     }
     
     [Serializable]
     public class AnalyticsSettings
     {
         [Serializable]
         public class AppsFlyerSettings
         {
             [SerializeField]
             private string _iosAppId;
                 
             [SerializeField]
             private string _iosDevKey;

             [SerializeField]
             private string _androidDevKey;

             [SerializeField]
             private bool _debug;
         
             internal string AppId => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? string.Empty
                     : _iosAppId;

             internal string DevKey => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidDevKey
                     : _iosDevKey;

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

             internal string AppId => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppId
                     : _iosAppId;

             internal DTDLogLevel LogLevel => _logLevel;
         }

         [SerializeField]
         private string _purchaseValidatorSlug;

         [SerializeField]
         private AppsFlyerSettings _appsFlyer;

         [SerializeField]
         private DevToDevSettings _devToDev;

         internal string PurchaseValidatorSlug => _purchaseValidatorSlug;
         internal AppsFlyerSettings AppsFlyer => _appsFlyer;
         internal DevToDevSettings DevToDev => _devToDev;
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
     
     private const string AssetFileName = "[Dreamsim] Settings";

     public static Settings Find()
     {
         return Resources.Load<Settings>(AssetFileName);
     }

     #if UNITY_EDITOR
     public static Settings Create()
     {
         var settings = CreateInstance<Settings>();
         UnityEditor.AssetDatabase.CreateAsset(settings, $"Assets/Dreamsim/Common/Resources/{AssetFileName}.asset");
         return settings;
     }
     #endif

     [SerializeField]
     private AnalyticsSettings _analytics;

     [SerializeField]
     private AdvertisementSettings _advertisement;

     [SerializeField]
     private GDPRSettings _gdpr;

     internal AnalyticsSettings Analytics => _analytics;
     internal AdvertisementSettings Advertisement => _advertisement;
     internal GDPRSettings GDPR => _gdpr;
 }
}