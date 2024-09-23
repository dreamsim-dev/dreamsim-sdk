using IngameDebugConsole;
using UnityEngine;
using UnityEngine.UI;

namespace Dreamsim
{
public class DebugConsoleCheatView : MonoBehaviour
{
    // Left button = 1; Center button = 2; Right button = 3;
    // The combination must be entered reversed (LLRLRRLRRC)
    private const string DefaultCombination = "0000000000"; // 0000000000
    private const string RightCombination = "2331331311"; //2331331311 1111111111

    [SerializeField]
    protected Button _leftButton;

    [SerializeField]
    protected Button _centerButton;

    [SerializeField]
    protected Button _rightButton;


    private string _combination = DefaultCombination;

    private void Awake()
    {
        _leftButton.onClick.AddListener(LeftButtonOnClickHandler);
        _centerButton.onClick.AddListener(CenterButtonOnClickHandler);
        _rightButton.onClick.AddListener(RightButtonOnClickHandler);
    }

    private void OnDestroy() { RemoveListenersFromButtons(); }

    private void OnEnable() { _combination = DefaultCombination; }

    private void RemoveListenersFromButtons()
    {
        _leftButton.onClick.RemoveListener(LeftButtonOnClickHandler);
        _centerButton.onClick.RemoveListener(
            CenterButtonOnClickHandler);
        _rightButton.onClick.RemoveListener(RightButtonOnClickHandler);
    }

    private void LeftButtonOnClickHandler() { SetValue(1); }

    private void CenterButtonOnClickHandler() { SetValue(2); }

    private void RightButtonOnClickHandler() { SetValue(3); }

    private void Check()
    {
        if (_combination != RightCombination) return;
        DreamsimCommon.DebugManager.ToggleConsole(true);
        RemoveListenersFromButtons();
    }

    private void SetValue(int value)
    {
        _combination = $"{value}{_combination}"[..10];
        Check();
    }
}
}