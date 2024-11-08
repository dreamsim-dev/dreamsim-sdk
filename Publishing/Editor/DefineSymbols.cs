using System.Linq;
using UnityEditor;

namespace Dreamsim.Publishing.Editor
{
    public static class DefineSymbols
    {
        public static void Add(string scriptingDefine)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            
            if (defineSymbolsForGroup.Contains(scriptingDefine)) return;
            
            var defines = defineSymbolsForGroup + ";" + scriptingDefine;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        public static void Remove(string scriptingDefine)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var list = defineSymbolsForGroup.Split(';').ToList();
            
            if (!list.Contains(scriptingDefine)) return;
            
            list.Remove(scriptingDefine);
            var defines = string.Join(";", list);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }
    }
}