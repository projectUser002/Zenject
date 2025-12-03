using Zenject;

public class PanelController : IUIState
{
    private readonly PanelView _view;
    private readonly SignalBus _signalBus;
    private readonly IFadeService _fadeService;
    private readonly ISoundPlayer _soundPlayer;
    private readonly Score _score;
    private readonly ISaver _saver;
    
    private bool _isActive;

    [Inject]
    public PanelController(PanelView view, SignalBus signalBus, IFadeService fadeService,
                         ISoundPlayer soundPlayer, Score score, ISaver saver)
    {
        _view = view;
        _signalBus = signalBus;
        _fadeService = fadeService;
        _soundPlayer = soundPlayer;
        _score = score;
        _saver = saver;
    }

    public void Enter()
    {
        if (_isActive) return;
        
        _isActive = true;
        _view.SubscribeOnClose(OnCloseButtonClicked);
        _view.SubscribeOnCollect(OnCollectButtonClicked);
        UpdateScoreText();
        
        _view.gameObject.SetActive(true);
        _fadeService.FadeIn(_view.PanelImage, 0.5f);
    }

    public void Exit()
    {
        if (!_isActive) return;
        
        _isActive = false;
        _view.UnsubscribeOnClose(OnCloseButtonClicked);
        _view.UnsubscribeOnCollect(OnCollectButtonClicked);
        
        SaveScore();
        _fadeService.FadeOut(_view.PanelImage, 0.3f);
        _view.gameObject.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        _soundPlayer.PlayCloseSound();
        _signalBus.Fire<ClosePanelSignal>();
    }

    private void OnCollectButtonClicked()
    {
        _score.AddScore(1);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _view.ScoreText.text = $"Score: {_score.CurrentScore}";
    }

    private void SaveScore()
    {
        _saver.SaveScore(_score.CurrentScore);
    }
}