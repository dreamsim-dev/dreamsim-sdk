using UnityEditor;
using UnityEngine;

namespace Dreamsim.Editor
{
    public class DreamsimSettingsWindow : EditorWindow
    {
        private DreamsimSettings _settings;
    
        [MenuItem("Window/Dreamsim/Settings")]
        public static void Editor_Settings()
        {
            var window = GetWindow(typeof(DreamsimSettingsWindow)) as DreamsimSettingsWindow;
            window!.titleContent = new GUIContent("Dreamsim");
            window.FindSettings();
        }

        private void OnGUI()
        {
            FindSettings();
         
            EditorGUI.BeginChangeCheck();
            var style = EditorStyles.boldLabel;
            style.fontSize = 18;
         
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Dreamsim Settings", style);
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
        }

        private void SeparateLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void FindSettings()
        {
            _settings = DreamsimSettings.Find();
            if (_settings == null)
            {
                _settings = DreamsimSettings.Create();
            }
        }
    }
}