using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dreamsim
{
public class DreamsimApp : MonoBehaviour
{
    private static DreamsimApp _instance;

    public static event Action OnInitialized;
    public static event Action OnInitializationFailed;
    
    [SerializeField]
    private Publishing.Analytics _analytics;

    [SerializeField]
    private Publishing.Advertisement _advertisement;

    [SerializeField]
    private DebugManager _debugManager;
    
    public static Publishing.Analytics Analytics => _instance._analytics;
    public static Publishing.Advertisement Advertisement => _instance._advertisement;
    public static DebugManager DebugManager => _instance._debugManager;

    public static bool IsInitialized { get; private set; }
    public static string AppVersionString { get; private set; }
    
    private void Start()
    {
        DreamsimLogger.Log($"Starting app. Version: {AppVersionString}");
        
        var settings = DreamsimSettings.Find();
        if (settings == null)
        {
            throw new NullReferenceException("Couldn't find DreamsimSettings instance. " +
                                             "Initialization won't continue");
        }
        
        RunAsync(settings).Forget();
    }

    private void Awake()
    {
        _instance = this;
        var buildNumberFile = Resources.Load<TextAsset>("[Dreamsim] BuildNumber");
        AppVersionString = $"{Application.version} ({buildNumberFile.text})";
    }

    private async UniTask RunAsync(DreamsimSettings settings)
    {
        var hasError = false;
        
        try
        {
            await _analytics.InitAsync(settings.Analytics, settings.GDPR);
            await _advertisement.InitAsync(settings.Advertisement);   
        }
        catch (Exception e)
        {
            hasError = true;
            DreamsimLogger.Log("Exception thrown during initialization");
            DreamsimLogger.LogException(e);
        }

        if (!hasError)
        {
            DreamsimLogger.Log("Initialized successfully");
            IsInitialized = true;
            OnInitialized?.Invoke();
        }
        else
        {
            OnInitializationFailed?.Invoke();
        }
    }
}
}