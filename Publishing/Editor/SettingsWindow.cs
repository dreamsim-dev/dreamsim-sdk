using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public class SettingsWindow : EditorWindow
{
    private const string Title = "Dreamsim Publishing Settings";

    private static SettingsWindow _instance;

    private Settings _settings;

    [MenuItem("Dreamsim/Publishing Settings")]
    public static void Editor_Settings() { OpenWindow(); }

    private static void OpenWindow()
    {
        const int w = 500;
        const int h = 800;
        _instance = GetWindow<SettingsWindow>();
        _instance!.titleContent = new GUIContent(Title);
        _instance.minSize = new Vector2(w, h);
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
        
        var style = EditorStyles.boldLabel;
        style.padding = new RectOffset(3, 0, 0, 0);
        style.fontSize = 19;
        style.fixedHeight = 22;
        style.alignment = TextAnchor.LowerLeft;

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(Title, style);
        SeparateLine();

        var settingsEditor = UnityEditor.Editor.CreateEditor(_settings);
        var settingsObject = settingsEditor.serializedObject;
        settingsObject.UpdateIfRequiredOrScript();

        var generalProp = settingsObject.FindProperty("_general");
        EditorGUILayout.PropertyField(generalProp);
        SeparateLine();

        EditorGUI.BeginChangeCheck();
        if (_settings.General.UseAnalytics)
        {
            var analyticsProp = settingsObject.FindProperty("_analytics");
            EditorGUILayout.PropertyField(analyticsProp);
            SeparateLine();
        }

        if (_settings.General.useAdvertisement)
        {
            var advertisementProp = settingsObject.FindProperty("_advertisement");
            EditorGUILayout.PropertyField(advertisementProp);
            SeparateLine();
        }

        var facebookProp = settingsObject.FindProperty("_facebook");
        EditorGUILayout.PropertyField(facebookProp);
        SeparateLine();

        var gdprProp = settingsObject.FindProperty("_gdpr");
        EditorGUILayout.PropertyField(gdprProp, GUILayout.ExpandHeight(true));
        SeparateLine();

        settingsObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();

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
        
        GUILayout.EndArea();
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

    private void FindSettings()
    {
        _settings = Settings.Find();
        if (_settings == null)
        {
            _settings = Settings.Create();
        }
    }
}
}