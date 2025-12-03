using UnityEngine;
using Zenject;

public class ObstacleInstaller : MonoInstaller
{
    [SerializeField] private DestructibleObstacle[] sceneObstacles;
    
    public override void InstallBindings()
    {
        foreach (var obstacle in sceneObstacles)
        {
            Container.InjectGameObject(obstacle.gameObject);
        }
    }
}