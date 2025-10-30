using GameAssembly.Core.Data;
using GameAssembly.Level;
using GameAssembly.Services;
using UnityEngine;

namespace GameAssembly.ScoresSystem
{
    public class ScoreCollectable : MonoBehaviour
    {
        [SerializeField] private MovingObject movingObject;
        [SerializeField] private float followSpeed;

        private Score _score;
        private LayersDataSO _layersDataSO;
        private Transform _followTarget;
        
        public void Init(Score score, LayersDataSO layersDataSO)
        {
            _score = score;
            _layersDataSO = layersDataSO;
        }

        private void Update()
        {
            if(!_followTarget)
                return;

            transform.position += (_followTarget.position - transform.position) * (followSpeed * Time.deltaTime);
        }

        private void Collect()
        {
            _score.AddScore();
            Destroy(gameObject);
        }

        public void StartFollowTarget(Transform target)
        {
            _followTarget = target;
            movingObject.enabled = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, _layersDataSO.PlayerLayer))
                return;
            
            Collect();
        }
    }
}