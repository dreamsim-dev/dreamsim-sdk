using System.Reflection;
using System.Xml.Linq;
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
        var gadSettings = Resources.Load("GoogleMobileAdsSettings");
        var gadSettingsType = gadSettings.GetType();
        gadSettingsType.GetField("adMobAndroidAppId", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(gadSettings,
            settings.Advertisement.AdMob.AndroidAppId);
        gadSettingsType.GetField("adMobIOSAppId", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(gadSettings,
            settings.Advertisement.AdMob.iOSAppId);
        EditorUtility.SetDirty(gadSettings);
    }

    private static XElement CreatePermissionElement(string name)
    {
        return new XElement(ManifestPermission,
            new XAttribute(XNamespace + "name", name));
    }
}
}