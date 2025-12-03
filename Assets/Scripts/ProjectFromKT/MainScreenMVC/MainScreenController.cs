using Zenject;

public class MainScreenController : IUIState
{
    private readonly MainScreenView _view;
    private readonly SignalBus _signalBus;
    private readonly ISoundPlayer _soundPlayer;

    [Inject]
    public MainScreenController(MainScreenView view, SignalBus signalBus, ISoundPlayer soundPlayer)
    {
        _view = view;
        _signalBus = signalBus;
        _soundPlayer = soundPlayer;
    }

    public void Enter()
    {
        _view.SubscribeOnOpen(OnOpenButtonClicked);
        _view.gameObject.SetActive(true);
    }

    public void Exit()
    {
        _view.UnsubscribeOnOpen(OnOpenButtonClicked);
        _view.gameObject.SetActive(false);
    }

    private void OnOpenButtonClicked()
    {
        _soundPlayer.PlayOpenSound();
        _signalBus.Fire<OpenPanelSignal>();
    }
}