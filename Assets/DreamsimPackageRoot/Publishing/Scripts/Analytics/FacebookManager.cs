using Facebook.Unity;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class FacebookManager : MonoBehaviour
{
    private static bool _isInitializingNow;
    private static bool _isActivated;

    private void OnApplicationPause(bool pause) { CheckActive(); }
    private void OnApplicationFocus(bool focus) { CheckActive(); }

    private void Start()
    {
        CheckActive();
    }

    private void CheckActive()
    {
        if (FB.IsInitialized)
        {
            if (_isActivated) return;
            _isActivated = true;
            FB.ActivateApp();
        }
        else if (!_isInitializingNow)
        {
            _isInitializingNow = true;
            DreamsimLogger.Log("Initializing Facebook SDK");
            FB.Init(() =>
            {
                _isInitializingNow = false;
                DreamsimLogger.Log("Activating Facebook SDK app");
                FB.ActivateApp();
            });
        }
    }
}
}