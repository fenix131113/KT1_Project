using UnityEngine;

namespace GameAssembly.EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float deadDistance;
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency;

        private Vector3 _startPosition;
        
        private void Awake()
        {
            _startPosition = transform.position;
            deadDistance = transform.position.x - deadDistance;
        }

        private void Update()
        {
            transform.position =  new Vector3(transform.position.x, _startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude, 0);
            
            if(transform.position.x <= deadDistance)
                Death();
        }

        private void Death() => Destroy(gameObject);
    }
}