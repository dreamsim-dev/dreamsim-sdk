#if UNITY_IOS
using Cysharp.Threading.Tasks;
using Unity.Advertisement.IosSupport;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class IOSConsentFlow : ConsentFlowBase
{
    public override async UniTask ProcessAsync()
    {
        if (Application.isEditor)
        {
            TrackingEnabled = true;
            return;
        }

        var attStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        if (attStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            await UniTask.WaitUntil(() =>
                ATTrackingStatusBinding.GetAuthorizationTrackingStatus()
                != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED);

            DreamsimLogger.Log("ATT requested");
        }
        else
        {
            DreamsimLogger.Log($"ATT status is: {attStatus}");
        }

        var requestDone = false;
        Application.RequestAdvertisingIdentifierAsync((advertisingId, trackingEnabled, errorMsg) =>
        {
            AdvertisingId = advertisingId;
            TrackingEnabled = ATTrackingStatusBinding.GetAuthorizationTrackingStatus()
                == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED;
            requestDone = true;
        });

        await UniTask.WaitUntil(() => requestDone);
    }
}
}
#endif