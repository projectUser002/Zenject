using Zenject;

public class GameSignalInstaller : Installer<GameSignalInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        Container.DeclareSignal<OpenPanelSignal>();
        Container.DeclareSignal<ClosePanelSignal>();
        Container.DeclareSignal<PlayerShootSignal>();
        Container.DeclareSignal<ObstacleDestroyedSignal>();
    }
}