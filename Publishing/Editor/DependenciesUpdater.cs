using System;
using System.Collections.Generic;
using System.Reflection;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public static class DependenciesUpdater
{
    public static void Update(Settings settings)
    {
        UpdateGADSettings(settings.Advertisement.AdMob.AndroidAppId, settings.Advertisement.AdMob.iOSAppId);
        UpdateFacebookSettings(settings.Facebook.AppLabel, settings.Facebook.AppId, settings.Facebook.ClientToken);
        UpdateMediationSettings(settings.Advertisement.Mediation);
        UpdateAndroidManifest(settings.Advertisement.AdMob.AndroidAppId);
    }

    private static void UpdateGADSettings(string androidAppId, string iOSAppId)
    {
        var gadSettings = Resources.Load("GoogleMobileAdsSettings");
        var gadSettingsType = gadSettings.GetType();

        gadSettingsType.GetField("adMobAndroidAppId", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(
            gadSettings,
            androidAppId);

        gadSettingsType.GetField("adMobIOSAppId", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(gadSettings,
            iOSAppId);

        EditorUtility.SetDirty(gadSettings);
    }

    private static void UpdateFacebookSettings(string appLabel, string appId, string clientToken)
    {
        FacebookSettings.AppLabels = new List<string> { appLabel };
        FacebookSettings.AppIds = new List<string> { appId };
        FacebookSettings.ClientTokens = new List<string> { clientToken };

        EditorUtility.SetDirty(FacebookSettings.Instance);
    }

    private static void UpdateMediationSettings(Settings.AdvertisementSettings.MediationType mediationType)
    {
        switch (mediationType)
        {
        case Settings.AdvertisementSettings.MediationType.LevelPlay:
            DefineSymbols.Add("DREAMSIM_USE_IRONSOURCE");
            DefineSymbols.Remove("DREAMSIM_USE_APPLOVIN");
            break;
        case Settings.AdvertisementSettings.MediationType.AppLovinMAX:
            DefineSymbols.Add("DREAMSIM_USE_APPLOVIN");
            DefineSymbols.Remove("DREAMSIM_USE_IRONSOURCE");
            break;
        case Settings.AdvertisementSettings.MediationType.None:
            DefineSymbols.Remove("DREAMSIM_USE_IRONSOURCE");
            DefineSymbols.Remove("DREAMSIM_USE_APPLOVIN");
            break;
        default: throw new ArgumentOutOfRangeException(nameof(mediationType), mediationType, null);
        }
    }

    private static void UpdateAndroidManifest(string adMobAppId)
    {
        AndroidManifestHelper.Update(adMobAppId);
    }
}
}