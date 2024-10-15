using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Facebook.Unity;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public static class DependenciesUpdater
{
    private const string ManifestPermission = "uses-permission";
    private static readonly XNamespace XNamespace = "http://schemas.android.com/apk/res/android";

    public static void Update(Settings settings)
    {
        UpdateGADSettings(settings.Advertisement.AdMob.AndroidAppId, settings.Advertisement.AdMob.iOSAppId);
        UpdateFacebookSettings(settings.Facebook.AppLabel, settings.Facebook.AppId, settings.Facebook.ClientToken);
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

    private static XElement CreatePermissionElement(string name)
    {
        return new XElement(ManifestPermission,
            new XAttribute(XNamespace + "name", name));
    }
}
}