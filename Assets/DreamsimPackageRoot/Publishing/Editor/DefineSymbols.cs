using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
    public static class DefineSymbols
    {
        public static void Add(string scriptingDefine)
        {
            var buildTargetGroup = Application.platform == RuntimePlatform.Android 
                ? BuildTargetGroup.Android : BuildTargetGroup.iOS;
            var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            
            if (defineSymbolsForGroup.Contains(scriptingDefine)) return;
            
            var defines = defineSymbolsForGroup + ";" + scriptingDefine;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        public static void Remove(string scriptingDefine)
        {
            var buildTargetGroup = Application.platform == RuntimePlatform.Android 
                ? BuildTargetGroup.Android : BuildTargetGroup.iOS;
            var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var list = defineSymbolsForGroup.Split(';').ToList();
            
            if (!list.Contains(scriptingDefine)) return;
            
            list.Remove(scriptingDefine);
            var defines = string.Join(";", list);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }
    }
}