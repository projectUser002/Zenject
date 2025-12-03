using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button collectButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image panelImage;

    private Action _onCloseAction;
    private Action _onCollectAction;

    public Button CloseButton => closeButton;
    public Button CollectButton => collectButton;
    public TextMeshProUGUI ScoreText => scoreText;
    public Image PanelImage => panelImage;

    public void SubscribeOnClose(Action action)
    {
        if (_onCloseAction != null)
        {
            closeButton.onClick.RemoveListener(OnCloseClicked);
        }
        
        _onCloseAction = action;
        if (action != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
    }

    public void UnsubscribeOnClose(Action action)
    {
        if (_onCloseAction == action)
        {
            closeButton.onClick.RemoveListener(OnCloseClicked);
            _onCloseAction = null;
        }
    }

    public void SubscribeOnCollect(Action action)
    {
        if (_onCollectAction != null)
        {
            collectButton.onClick.RemoveListener(OnCollectClicked);
        }
        
        _onCollectAction = action;
        if (action != null)
        {
            collectButton.onClick.AddListener(OnCollectClicked);
        }
    }

    public void UnsubscribeOnCollect(Action action)
    {
        if (_onCollectAction == action)
        {
            collectButton.onClick.RemoveListener(OnCollectClicked);
            _onCollectAction = null;
        }
    }

    private void OnCloseClicked()
    {
        _onCloseAction?.Invoke();
    }

    private void OnCollectClicked()
    {
        _onCollectAction?.Invoke();
    }
}