using Dreamsim;
using Dreamsim.Publishing;
using UnityEngine;

public class Test : MonoBehaviour
{
    private async void Start()
    {
        DreamsimCommon.Create();
        DreamsimPublishing.Create();
        await DreamsimPublishing.InitAsync();
    }
}
