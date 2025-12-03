using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button openButton;
    
    private Action _onOpenAction;

    public Button OpenButton => openButton;

    public void SubscribeOnOpen(Action action)
    {
        if (_onOpenAction != null)
        {
            openButton.onClick.RemoveListener(OnOpenClicked);
        }
        
        _onOpenAction = action;
        if (action != null)
        {
            openButton.onClick.AddListener(OnOpenClicked);
        }
    }

    public void UnsubscribeOnOpen(Action action)
    {
        if (_onOpenAction == action)
        {
            openButton.onClick.RemoveListener(OnOpenClicked);
            _onOpenAction = null;
        }
    }
    
    private void OnOpenClicked()
    {
        _onOpenAction?.Invoke();
    }
}