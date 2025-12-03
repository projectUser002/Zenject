using System;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 1f;
    
    private IMemoryPool _pool;
    private float _lifeTimer;
    private float _lifetime;
    private bool _isActive;

    public class Factory : PlaceholderFactory<Bullet> { }
    
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Bullet> { }
    
    public void Initialize(float lifetime, IMemoryPool pool)
    {
        _lifetime = lifetime;
        _pool = pool;
        _lifeTimer = 0f;
        _isActive = true;
    }
    
    private void Update()
    {
        if (!_isActive) return;
        
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= _lifetime)
        {
            Despawn();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;
        Debug.Log("Trigger");
        DestructibleObstacle obstacle = other.GetComponent<DestructibleObstacle>();
        if (obstacle != null)
        {
            Debug.Log("Trigger Active");
            obstacle.TakeDamage(damage);
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
}