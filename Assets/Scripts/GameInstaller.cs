using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip destructionSound;

    [Header("UI")]
    [SerializeField] private MainScreenView mainScreenView;
    [SerializeField] private PanelView panelView;

    [Header("Player")]
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    [Header("Bullet")]
    [SerializeField] private Bullet bulletPrefab;

    [Header("Obstacles")]
    [SerializeField] private DestructibleObstacle[] sceneObstacles;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<OpenPanelSignal>();
        Container.DeclareSignal<ClosePanelSignal>();
        Container.DeclareSignal<PlayerShootSignal>();
        Container.DeclareSignal<ObstacleDestroyedSignal>();

        Container.Bind<MainScreenView>().FromInstance(mainScreenView).AsSingle();
        Container.Bind<PanelView>().FromInstance(panelView).AsSingle();

        Container.Bind<IFadeService>().To<FadeService>().AsSingle();
        Container.Bind<ISoundPlayer>().To<SoundPlayer>().AsSingle()
            .WithArguments(audioSource, openSound, closeSound, shootSound, destructionSound);
        Container.Bind<ISaver>().To<JsonSaver>().AsSingle();

        Container.Bind<Score>().AsSingle();

        Container.Bind<PlayerController>()
            .FromComponentInNewPrefab(playerPrefab)
            .WithGameObjectName("Player")
            .UnderTransform(playerSpawnPoint)
            .AsSingle()
            .NonLazy();

        Container.BindMemoryPool<Bullet, Bullet.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(bulletPrefab)
            .UnderTransformGroup("Bullets");

        Container.BindFactory<Bullet, Bullet.Factory>().FromPoolableMemoryPool<Bullet>(poolBinder =>
            poolBinder.WithInitialSize(20)
                .FromComponentInNewPrefab(bulletPrefab)
                .UnderTransformGroup("Bullets"));

        var obstacles = FindObjectsOfType<DestructibleObstacle>();
        foreach (var obstacle in obstacles)
        {
            Container.InjectGameObject(obstacle.gameObject);
        }
    }
}