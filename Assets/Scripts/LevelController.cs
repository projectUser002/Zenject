using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float tileHeight = 1f;
    [SerializeField] private int poolSize = 10;
    
    [Header("Obstacle Settings")]
    [SerializeField] private float obstacleSpawnInterval = 2f;
    [SerializeField] private float[] obstacleLanes = { -2f, 0f, 2f };
    
    private float _spawnTimer;
    private DestructibleObstacle.Factory _obstacleFactory;
    private Transform _player;
    
    [Inject]
    public void Construct(DestructibleObstacle.Factory obstacleFactory, PlayerController player)
    {
        _obstacleFactory = obstacleFactory;
        _player = player.transform;
    }
    
    private void Start()
    {
        InitializeTilemap();
    }
    
    private void Update()
    {
        ScrollTilemap();
        HandleObstacleSpawning();
    }
    
    private void InitializeTilemap()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3Int position = new Vector3Int(0, -i, 0);
            tilemap.SetTile(position, GetRandomTile());
        }
    }
    
    private void ScrollTilemap()
    {
        tilemap.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        
        Bounds bounds = tilemap.localBounds;
        if (bounds.max.y < -tileHeight)
        {
            RecycleTiles();
        }
    }
    
    private void RecycleTiles()
    {
  
    }
    
    private void HandleObstacleSpawning()
    {
        _spawnTimer += Time.deltaTime;
        
        if (_spawnTimer >= obstacleSpawnInterval)
        {
            SpawnObstacle();
            _spawnTimer = 0f;
        }
    }
    
    private void SpawnObstacle()
    {
        float laneX = obstacleLanes[Random.Range(0, obstacleLanes.Length)];
        float spawnY = _player.position.y + 10f;
        
        Vector3 spawnPosition = new Vector3(laneX, spawnY, 0);
        
        DestructibleObstacle obstacle = _obstacleFactory.Create();
        obstacle.transform.position = spawnPosition;
    }
    
    private TileBase GetRandomTile()
    {
        return tilemap.GetTile(new Vector3Int(0, 0, 0));
    }
}