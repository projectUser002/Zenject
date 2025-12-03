using UnityEngine;
using Zenject;

public class UISwitcher : MonoBehaviour
{
    private IUIState _currentState;
    private MainScreenController _mainScreenController;
    private PanelController _panelController;
    private UIFactory _uiFactory;
    
    [Inject]
    public void Construct(MainScreenController mainScreenController, 
                         PanelController panelController)
    {
        _mainScreenController = mainScreenController;
        _panelController = panelController;
    }
    
    private void Start()
    {
        SwitchToMainScreen();
    }
    
    private void SwitchToMainScreen()
    {
        SwitchState(_mainScreenController);
    }
    
    private void SwitchToPanel()
    {
        SwitchState(_panelController);
    }
    
    private void SwitchState(IUIState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}