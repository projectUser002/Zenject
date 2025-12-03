using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float horizontalLimit = 8f;
    [SerializeField] private float verticalLimit = 4f;
    
    [Header("Shooting")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private float shootCooldown = 0.2f;
    
    private float _lastShootTime;
    private Bullet.Pool _bulletPool;
    private ISoundPlayer _soundPlayer;
    private SignalBus _signalBus;
    private Rigidbody2D _rigidbody;
    
    [Inject]
    public void Construct(Bullet.Pool bulletPool, ISoundPlayer soundPlayer, SignalBus signalBus)
    {
        _bulletPool = bulletPool;
        _soundPlayer = soundPlayer;
        _signalBus = signalBus;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0f;
            _rigidbody.freezeRotation = true;
        }
    }
    
    private void Update()
    {
        HandleShooting();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        // Получаем ввод
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Создаем вектор движения
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        
        // Нормализуем, если движение по диагонали быстрее
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        // Применяем скорость
        Vector2 velocity = movement * moveSpeed;
        
        // Учитываем коллизии через Rigidbody
        _rigidbody.velocity = velocity;
        
        // Ограничиваем позицию в пределах экрана
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -horizontalLimit, horizontalLimit);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -verticalLimit, verticalLimit);
        transform.position = clampedPosition;
        
        // Альтернативный способ с MovePosition:
        // Vector2 newPosition = _rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime;
        // newPosition.x = Mathf.Clamp(newPosition.x, -horizontalLimit, horizontalLimit);
        // newPosition.y = Mathf.Clamp(newPosition.y, -verticalLimit, verticalLimit);
        // _rigidbody.MovePosition(newPosition);
    }
    
    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time - _lastShootTime > shootCooldown)
        {
            Shoot();
            _lastShootTime = Time.time;
        }
    }
    
    private void Shoot()
    {
        Bullet bullet = _bulletPool.Spawn(_bulletPool);
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.Initialize(bulletLifetime , _bulletPool);
        
        _soundPlayer.PlayShootSound();
        _signalBus.Fire<PlayerShootSignal>();
    }
}