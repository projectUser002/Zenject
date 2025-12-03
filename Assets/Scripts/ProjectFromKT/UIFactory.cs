using Zenject;

public class UIFactory
{
    private readonly DiContainer _container;
    
    public UIFactory(DiContainer container)
    {
        _container = container;
    }
    
    public MainScreenController CreateMainScreenController()
    {
        return _container.Instantiate<MainScreenController>();
    }
    
    public PanelController CreatePanelController()
    {
        return _container.Instantiate<PanelController>();
    }
}