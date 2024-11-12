using System.Linq;
using UnityEditor;

namespace Dreamsim.Publishing.Editor
{
public static class DefineSymbols
{
    public static void Add(string symbol)
    {
        Add(symbol, BuildTargetGroup.Android);
        Add(symbol, BuildTargetGroup.iOS);
    }

    public static void Remove(string symbol)
    {
        Remove(symbol, BuildTargetGroup.Android);
        Remove(symbol, BuildTargetGroup.iOS);
    }

    private static void Add(string symbol, BuildTargetGroup targetGroup)
    {
        var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (defineSymbolsForGroup.Contains(symbol)) return;
        var defines = defineSymbolsForGroup + ";" + symbol;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
    }

    private static void Remove(string symbol, BuildTargetGroup targetGroup)
    {
        var defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        var list = defineSymbolsForGroup.Split(';').ToList();
        if (!list.Contains(symbol)) return;
        list.Remove(symbol);
        var defines = string.Join(";", list);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
    }
}
}