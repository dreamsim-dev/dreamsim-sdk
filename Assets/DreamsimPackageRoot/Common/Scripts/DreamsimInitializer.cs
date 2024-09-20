using UnityEngine;

namespace Dreamsim
{
    public static class DreamsimInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AfterSceneLoad()
        {
            var prefab = Resources.Load("Prefabs/[Dreamsim] App");
            var go = Object.Instantiate(prefab) as GameObject;
            go!.name = prefab.name;
            Object.DontDestroyOnLoad(go);
        }
    }
}