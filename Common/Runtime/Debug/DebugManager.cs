using UnityEngine;

namespace Dreamsim
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        private Canvas _consoleCanvas;

        public bool IsConsoleActive => _consoleCanvas.enabled;

        private void Start()
        {
            ToggleConsole(false);
        }

        public void ToggleConsole(bool value)
        {
            _consoleCanvas.enabled = value;
        }
    }
}