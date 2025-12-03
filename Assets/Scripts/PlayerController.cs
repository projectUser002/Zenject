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
    private Bullet.Factory _bulletFactory;
    private ISoundPlayer _soundPlayer;
    private SignalBus _signalBus;
    private Rigidbody2D _rigidbody;
    private Target _target;
    
    [Inject]
    public void Construct(Bullet.Factory bulletFactory, ISoundPlayer soundPlayer, 
                         SignalBus signalBus, Target target)
    {
        _bulletFactory = bulletFactory;
        _soundPlayer = soundPlayer;
        _signalBus = signalBus;
        _target = target;

        _target.SetPlayer(transform);
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        Vector2 velocity = movement * moveSpeed;
        _rigidbody.velocity = velocity;
        
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -horizontalLimit, horizontalLimit);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -verticalLimit, verticalLimit);
        transform.position = clampedPosition;
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
        Bullet bullet = _bulletFactory.Create();
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;

        var pool = _bulletFactory as IMemoryPool<Bullet>;
        bullet.Initialize(bulletLifetime, pool);
        
        _soundPlayer.PlayShootSound();
        _signalBus.Fire<PlayerShootSignal>();
    }
}