using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Dreamsim.Publishing
{
    public class FirebaseManager : MonoBehaviour
    {
        private FirebaseApp _app;
        
        public FirebaseApp App => _app;
        
        internal async UniTask InitAsync()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var status = await FirebaseApp.CheckAndFixDependenciesAsync();
                
                if (status == DependencyStatus.Available) Init();
                else DreamsimLogger.LogError("Could not resolve all Firebase dependencies: " + status);
            }
            else Init();
        }

        private void Init()
        {
            _app = FirebaseApp.DefaultInstance;
        }
    }
}