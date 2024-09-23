using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Dreamsim.Publishing
{
public class DreamsimPublishing : MonoBehaviour
{
    private static DreamsimPublishing _instance;

    [SerializeField]
    private Analytics _analytics;

    [SerializeField]
    private Advertisement _advertisement;

    public static Analytics Analytics => _instance._analytics;
    public static Advertisement Advertisement => _instance._advertisement;
    
    private Settings _settings;

    public static void Create()
    {
        var prefab = Resources.Load("Dreamsim/[Dreamsim] Publishing");
        _instance = (Instantiate(prefab) as GameObject)!.GetComponent<DreamsimPublishing>();
        _instance!.name = prefab.name;
        DontDestroyOnLoad(_instance);
    }

    public static async UniTask InitAsync()
    {
        _instance._settings = Settings.Find();
        if (_instance._settings == null)
        {
            throw new NullReferenceException("Couldn't find DreamsimSettings instance. "
                + "Initialization won't continue");
        }

        var hasError = false;
        
        try
        {
            await Analytics.InitAsync(_instance._settings.Analytics, _instance._settings.GDPR);
            await Advertisement.InitAsync(_instance._settings.Advertisement);
        }
        catch (Exception e)
        {
            hasError = true;
            var error = e.Message;
            DreamsimLogger.Log($"Error occured during initialization\n{error}");
        }
        
        if (!hasError)
        {
            DreamsimLogger.Log("Analytics initialized successfully");
        }
    }
}
}
