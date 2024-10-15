using System.Collections.Generic;
using UnityEngine;

namespace Dreamsim.Publishing
{
public class CrossPromoHelper : MonoBehaviour
{
    public void AttributeAndOpenStore(string appId, string campaign, List<EventParam> eventParams)
    {
        DreamsimPublishing.Analytics.AppsFlyerManager.AttributeAndOpenStore(appId, campaign, eventParams);
    }
}
}