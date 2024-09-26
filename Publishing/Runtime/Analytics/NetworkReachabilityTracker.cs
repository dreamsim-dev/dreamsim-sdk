using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class NetworkReachabilityTracker : MonoBehaviour
{
    private const string Url = "https://dreamsim.dev/api/healthcheck";

    [SerializeField]
    private float _timeInterval;

    private bool _firstTime = true;
    private bool _lastResult;
    private bool _done;

    public void Run() { RunAsync().Forget(); }

    private void OnDestroy() { _done = true; }

    private async UniTask RunAsync()
    {
        while (!_done)
        {
            var result = await PingAsync();
            if (_firstTime || _lastResult != result)
            {
                _firstTime = false;
                _lastResult = result;
                DreamsimPublishing.Analytics.LogNetworkReachability(result);
            }

            await UniTask.WaitForSeconds(_timeInterval, ignoreTimeScale: true);
        }
    }

    private async UniTask<bool> PingAsync()
    {
        var response = await Client.SendGetAsync(Url);
        return response.IsSuccessStatusCode;
    }
}
}