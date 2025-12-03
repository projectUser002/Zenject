using UnityEngine;
using Zenject;

public class Target : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float horizontalOffset = 8f;
    [SerializeField] private float followSmoothness = 5f;
    [SerializeField] private float detectionRadius = 5f;
    
    private Transform _playerTransform;
    private Vector3 _targetPosition;
    
    public float DetectionRadius => detectionRadius;
    public Vector3 Position => transform.position;
    
    public void SetPlayer(Transform player)
    {
        _playerTransform = player;
    }
    
    private void Update()
    {
        if (_playerTransform != null)
        {
            _targetPosition = new Vector3(
                _playerTransform.position.x + horizontalOffset,
                _playerTransform.position.y,
                _playerTransform.position.z
            );
            
            transform.position = Vector3.Lerp(
                transform.position, 
                _targetPosition, 
                followSmoothness * Time.deltaTime
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}