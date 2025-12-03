using UnityEngine;
using Zenject;

public class SmartBullet : MonoBehaviour, IPoolable<IMemoryPool>
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Targeting")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    
    private IMemoryPool _pool;
    private float _lifeTimer;
    private float _lifetime;
    private bool _isActive;

    private Target _target;
    private Transform _player;
    
    private Transform _currentTarget;
    private bool _hasObstacleTarget;

    public class Factory : PlaceholderFactory<SmartBullet> { }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, SmartBullet> { }
    
    [Inject]
    public void Construct(Target target, PlayerController player)
    {
        _target = target;
        _player = player.transform;
    }
    
    public void Initialize(float lifetime, IMemoryPool pool)
    {
        _lifetime = lifetime;
        _pool = pool;
        _lifeTimer = 0f;
        _isActive = true;
        
        FindTarget();
    }
    
    private void FindTarget()
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(
            transform.position, 
            detectionRadius, 
            obstacleLayer
        );
        
        if (obstacles.Length > 0)
        {
            Transform closestObstacle = null;
            float closestDistance = float.MaxValue;
            
            foreach (var obstacle in obstacles)
            {
                float distance = Vector3.Distance(transform.position, obstacle.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObstacle = obstacle.transform;
                }
            }
            
            _currentTarget = closestObstacle;
            _hasObstacleTarget = true;
        }
        else
        {
            _currentTarget = _target.transform;
            _hasObstacleTarget = false;
        }
    }
    
    private void Update()
    {
        if (!_isActive) return;
        
        MoveTowardsTarget();
        
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= _lifetime)
        {
            Despawn();
        }
    }
    
    private void MoveTowardsTarget()
    {
        if (_currentTarget == null)
        {

            transform.Translate(Vector3.up * speed * Time.deltaTime);
            return;
        }
        

        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
        

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;
        
        DestructibleObstacle obstacle = other.GetComponent<DestructibleObstacle>();
        if (obstacle != null)
        {
            obstacle.TakeDamage(1f);
        }
        
        Despawn();
    }
    
    private void Despawn()
    {
        if (_isActive)
        {
            _isActive = false;
            _pool.Despawn(this);
        }
    }
    
    public void OnDespawned()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }
    
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        gameObject.SetActive(true);
        _isActive = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}