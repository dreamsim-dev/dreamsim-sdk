using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public class SettingsWindow : EditorWindow
{
    private const string Title = "Dreamsim Publishing Settings";

    private static SettingsWindow _instance;

    private static Settings _settings;

    [MenuItem("Dreamsim/Publishing Settings")]
    public static void Editor_Settings() { OpenWindow(); }

    private static void OpenWindow()
    {
        const int w = 500;
        const int h = 750;
        _instance = GetWindow<SettingsWindow>();
        _instance!.titleContent = new GUIContent(Title);
        _instance.minSize = new Vector2(w, h);
    }

    private static void H1(string text)
    {
        var style = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 20,
            fixedHeight = 22,
            alignment = TextAnchor.MiddleLeft
        };

        EditorGUILayout.Space(-1);
        EditorGUILayout.LabelField(text, style);
        EditorGUILayout.Space(2);
    }

    private static void H2(string text)
    {
        var style = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            fixedHeight = 20,
            alignment = TextAnchor.MiddleLeft
        };

        EditorGUILayout.Space(2);
        EditorGUILayout.LabelField(text, style);
        EditorGUILayout.Space(5);
    }

    private static void Tip(string text, string link = null)
    {
        var style = new GUIStyle(GUI.skin.GetStyle("HelpBox")) { richText = true };

        if (!string.IsNullOrWhiteSpace(link))
        {
            if (GUILayout.Button("\u2191" + text, style)) Application.OpenURL(link);
        }
        else
        {
            EditorGUILayout.LabelField("\u2191" + text, style);
        }

        EditorGUILayout.Space(2);
    }

    private void OnGUI()
    {
        // Your GUI Code
        FindSettings();

        var padding = new RectOffset(0, 0, 0, 0);
        var area = new Rect(padding.right,
            padding.top,
            position.width - (padding.right + padding.left),
            position.height - (padding.top + padding.bottom));

        GUILayout.BeginArea(area);

        EditorGUILayout.Separator();
        H1(Title);
        SeparateLine();

        var settingsEditor = UnityEditor.Editor.CreateEditor(_settings);
        var settingsObject = settingsEditor.serializedObject;
        settingsObject.UpdateIfRequiredOrScript();

        EditorGUI.BeginChangeCheck();

        GeneralArea(settingsObject);
        if (_settings.General.UseAnalytics) AnalyticsArea(settingsObject);
        if (_settings.General.useAdvertisement) AdvertisementArea(settingsObject);
        FacebookArea(settingsObject);
        GDPRArea(settingsObject);
        ButtonsArea();

        settingsObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();

        GUILayout.EndArea();
    }

    private void GeneralArea(SerializedObject settingsObject)
    {
        H2("General");

        var iosStoreAppIdProp = settingsObject.FindProperty("_general._iosStoreAppId");
        EditorGUILayout.PropertyField(iosStoreAppIdProp);
        Tip("Without \"id\" part");
        var useAnalyticsProp = settingsObject.FindProperty("_general._useAnalytics");
        EditorGUILayout.PropertyField(useAnalyticsProp);
        var useAdvertisementProp = settingsObject.FindProperty("_general._useAdvertisement");
        EditorGUILayout.PropertyField(useAdvertisementProp);
        SeparateLine();
    }

    private void AnalyticsArea(SerializedObject settingsObject)
    {
        H2("Analytics");

        var purchaseValidationSlugProp = settingsObject.FindProperty("_analytics._purchaseValidatorSlug");
        EditorGUILayout.PropertyField(purchaseValidationSlugProp);
        Tip("Provided by the SDK's team");
        var appsFlyerProp = settingsObject.FindProperty("_analytics._appsFlyer");
        EditorGUILayout.PropertyField(appsFlyerProp);
        var devToDevProp = settingsObject.FindProperty("_analytics._devToDev");
        EditorGUILayout.PropertyField(devToDevProp);
        SeparateLine();
    }

    private void AdvertisementArea(SerializedObject settingsObject)
    {
        H2("Advertisement");

        var mediationProp = settingsObject.FindProperty("_advertisement._mediation");
        EditorGUILayout.PropertyField(mediationProp);
        var useRewardedVideoProp = settingsObject.FindProperty("_advertisement._useRewardedVideo");
        EditorGUILayout.PropertyField(useRewardedVideoProp);

        var levelPlayProp = settingsObject.FindProperty("_advertisement._levelPlay");
        if (_settings.Advertisement.Mediation == Settings.AdvertisementSettings.MediationType.LevelPlay)
        {
            EditorGUILayout.PropertyField(levelPlayProp);
        }

        var appLovinProp = settingsObject.FindProperty("_advertisement._appLovin");
        if (_settings.Advertisement.Mediation == Settings.AdvertisementSettings.MediationType.AppLovinMAX)
        {
            EditorGUILayout.PropertyField(appLovinProp);
        }

        var adMobProp = settingsObject.FindProperty("_advertisement._adMob");
        EditorGUILayout.PropertyField(adMobProp);
        SeparateLine();
    }

    private void FacebookArea(SerializedObject settingsObject)
    {
        H2("Facebook");

        var appLabelProp = settingsObject.FindProperty("_facebook._appLabel");
        EditorGUILayout.PropertyField(appLabelProp);
        var appIdProp = settingsObject.FindProperty("_facebook._appId");
        EditorGUILayout.PropertyField(appIdProp);
        var clientTokenProp = settingsObject.FindProperty("_facebook._clientToken");
        EditorGUILayout.PropertyField(clientTokenProp);
        SeparateLine();
    }

    private void GDPRArea(SerializedObject settingsObject)
    {
        H2("GDPR");

        var googleMobileAdsTestDeviceHashedIdsProp =
            settingsObject.FindProperty("_gdpr._googleMobileAdsTestDeviceHashedIds");
        EditorGUILayout.PropertyField(googleMobileAdsTestDeviceHashedIdsProp);
        const string link = "https://developers.google.com/admob/unity/privacy#testing";
        Tip($"How to obtain: <a href=\"{link}\">{link}</a>", link);
        SeparateLine();
    }
    
    private void ButtonsArea()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Update Dependencies", GUILayout.Height(30), GUILayout.Width(200)))
        {
            DependenciesUpdater.Update(_settings);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    private void SeparateLine()
    {
        EditorGUILayout.Separator();
        var rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        rect.x -= 10;
        rect.width += 20;
        EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f));
    }

    private static void FindSettings()
    {
        _settings = Settings.Find();
        if (_settings == null)
        {
            _settings = Settings.Create();
        }
    }
}
}