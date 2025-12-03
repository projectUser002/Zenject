using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DestructibleObstacle : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth;
    
    [Header("Visuals")]
    [SerializeField] private GameObject intactVisual;
    [SerializeField] private GameObject destroyedVisual;
    [SerializeField] private List<Rigidbody2D> debrisPieces;
    
    [Header("Collision")]
    [SerializeField] private Collider2D mainCollider;
    [SerializeField] private bool disablePlayerCollision = true;
    
    private ISoundPlayer _soundPlayer;
    private SignalBus _signalBus;
    
    [Inject]
    public void Construct(ISoundPlayer soundPlayer, SignalBus signalBus)
    {
        _soundPlayer = soundPlayer;
        _signalBus = signalBus;
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        SetVisualState(true);
        foreach (var debris in debrisPieces)
        {
            debris.gameObject.SetActive(false);
            if (disablePlayerCollision)
            {
                var debrisCollider = debris.GetComponent<Collider2D>();
                if (debrisCollider != null)
                {
                    Physics2D.IgnoreCollision(debrisCollider, 
                        FindObjectOfType<PlayerController>().GetComponent<Collider2D>());
                }
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            DestroyObstacle();
        }
    }
    
    private void DestroyObstacle()
    {
        _soundPlayer.PlayDestructionSound();
        _signalBus.Fire<ObstacleDestroyedSignal>();
        
        if (mainCollider != null)
            mainCollider.enabled = false;
        
        SetVisualState(false);
        
        foreach (var debris in debrisPieces)
        {
            debris.gameObject.SetActive(true);
            debris.AddForce(new Vector2(
                Random.Range(-5f, 5f),
                Random.Range(2f, 8f)
            ), ForceMode2D.Impulse);
            debris.AddTorque(Random.Range(-10f, 10f));
        }
        
        Destroy(gameObject, 5f);
    }
    
    private void SetVisualState(bool intact)
    {
        if (intactVisual != null)
            intactVisual.SetActive(intact);
        
        if (destroyedVisual != null)
            destroyedVisual.SetActive(!intact);
    }
    
    public class Factory : PlaceholderFactory<DestructibleObstacle> { }
}