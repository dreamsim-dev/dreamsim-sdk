using System;
using System.Collections.Generic;
using DevToDev.Analytics;
using UnityEngine;

namespace Dreamsim
{
 public class DreamsimSettings : ScriptableObject
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
             
             public string AppKey => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppKey
                     : _iosAppKey;

             public bool UseRewardedVideo => _useRewardedVideo;
         }

         [SerializeField]
         private LevelPlaySettings _levelPlay;

         public LevelPlaySettings LevelPlay => _levelPlay;
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
         
             public string AppId => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? string.Empty
                     : _iosAppId;

             public string DevKey => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidDevKey
                     : _iosDevKey;

             public bool Debug => _debug;
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

             public string AppId => Application.isEditor
                 ? string.Empty
                 : Application.platform == RuntimePlatform.Android
                     ? _androidAppId
                     : _iosAppId;

             public DTDLogLevel LogLevel => _logLevel;
         }

         [SerializeField]
         private string _purchaseValidatorSlug;

         [SerializeField]
         private AppsFlyerSettings _appsFlyer;

         [SerializeField]
         private DevToDevSettings _devToDev;

         public string PurchaseValidatorSlug => _purchaseValidatorSlug;
         public AppsFlyerSettings AppsFlyer => _appsFlyer;
         public DevToDevSettings DevToDev => _devToDev;
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
         
         public List<string> GoogleMobileAdsTestDeviceHashedIds => _googleMobileAdsTestDeviceHashedIds;
     }
     
     private const string AssetFileName = "[Dreamsim] Settings";

     public static DreamsimSettings Find()
     {
         return Resources.Load<DreamsimSettings>(AssetFileName);
     }

     #if UNITY_EDITOR
     public static DreamsimSettings Create()
     {
         var settings = CreateInstance<DreamsimSettings>();
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

     public AnalyticsSettings Analytics => _analytics;
     public AdvertisementSettings Advertisement => _advertisement;
     public GDPRSettings GDPR => _gdpr;
 }
}