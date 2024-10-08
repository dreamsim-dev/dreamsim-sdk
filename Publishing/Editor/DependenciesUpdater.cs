using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public static class DependenciesUpdater
{
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
}
}