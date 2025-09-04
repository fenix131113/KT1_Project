using System;
using GameAssembly.Level;
using GameAssembly.Services;
using UnityEngine;

namespace GameAssembly.ScoresSystem
{
    public class ScoreCollectable : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private MovingObject movingObject;
        [SerializeField] private float followSpeed;

        private Scores _scores;
        private Transform _followTarget;
        
        public void Init(Scores scores) => _scores = scores;

        private void Update()
        {
            if(!_followTarget)
                return;

            transform.position += (_followTarget.position - transform.position) * (followSpeed * Time.deltaTime);
        }

        private void Collect()
        {
            _scores.AddScore();
            Destroy(gameObject);
        }

        public void StartFollowTarget(Transform target)
        {
            _followTarget = target;
            movingObject.enabled = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, playerLayer))
                return;
            
            Collect();
        }
    }
}