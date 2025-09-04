using GameAssembly.Services;
using UnityEngine;

namespace GameAssembly.ScoresSystem
{
    public class ScoreGravityZone : MonoBehaviour
    {
        [SerializeField] private ScoreCollectable score;
        [SerializeField] private LayerMask playerLayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, playerLayer))
                return;
            
            score.StartFollowTarget(other.transform);
            gameObject.SetActive(false);
        }
    }
}