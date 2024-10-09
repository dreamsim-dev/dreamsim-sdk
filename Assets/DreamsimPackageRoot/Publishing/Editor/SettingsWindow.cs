using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public class SettingsWindow : EditorWindow
{
    private const string Title = "Dreamsim Publishing Settings";

    private Settings _settings;

    [MenuItem("Dreamsim/Publishing Settings")]
    public static void Editor_Settings()
    {
        var window = GetWindow(typeof(SettingsWindow)) as SettingsWindow;
        window!.titleContent = new GUIContent(Title);
        window.FindSettings();
    }

    private void OnGUI()
    {
        FindSettings();

        EditorGUI.BeginChangeCheck();
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

        var analyticsProp = settingsObject.FindProperty("_analytics");
        EditorGUILayout.PropertyField(analyticsProp);
        SeparateLine();

        var advertisementProp = settingsObject.FindProperty("_advertisement");
        EditorGUILayout.PropertyField(advertisementProp);
        SeparateLine();

        var gdprProp = settingsObject.FindProperty("_gdpr");
        EditorGUILayout.PropertyField(gdprProp);

        settingsObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
        SeparateLine();
        GUILayout.Space(10);

        if (GUILayout.Button("Update Dependencies"))
        {
            DependenciesUpdater.Update(_settings);
        }
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