using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
[CustomEditor(typeof(Settings))]
public class SettingsEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Settings"))
        {
            SettingsWindow.Open();
        }
    }
}
}