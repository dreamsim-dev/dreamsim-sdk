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

    [SerializeField]
    private CrossPromoHelper _crossPromoHelper;

    public static Analytics Analytics => _instance._analytics;
    public static Advertisement Advertisement => _instance._advertisement;
    public static CrossPromoHelper CrossPromo => _instance._crossPromoHelper;

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

        var settings = _instance._settings;

        var hasError = false;

        try
        {
            if (settings.General.UseAnalytics)
            {
                await Analytics.InitAsync(settings.General.StoreAppId,
                    settings.Analytics,
                    settings.GDPR);
            }

            if (settings.General.useAdvertisement) await Advertisement.InitAsync(settings.Advertisement);
        }
        catch (Exception e)
        {
            hasError = true;
            var error = e.Message;
            DreamsimLogger.LogError("Error occured during initialization. See next error log");
            DreamsimLogger.LogError(error.Trim());
        }

        if (!hasError)
        {
            var storeAppId = settings.General.StoreAppId;
            DreamsimLogger.Log($"Publishing initialized ({storeAppId})");
        }
    }
}
}