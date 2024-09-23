using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dreamsim
{
public class DreamsimCommon : MonoBehaviour
{
    private static DreamsimCommon _instance;

    [SerializeField]
    private DebugManager _debugManager;

    public static DebugManager DebugManager => _instance._debugManager;

    public static string AppVersionString { get; private set; }

    public static void Create()
    {
        var prefab = Resources.Load("Dreamsim/[Dreamsim] Common");
        _instance = (Instantiate(prefab) as GameObject)!.GetComponent<DreamsimCommon>();
        _instance!.name = prefab.name;
        DontDestroyOnLoad(_instance);
        
        var buildNumberFile = Resources.Load<TextAsset>("[Dreamsim] BuildNumber");
        AppVersionString = $"{Application.version} ({buildNumberFile.text})";
        
        DreamsimLogger.Log($"Starting app. Version: {AppVersionString}");
    }
}
}