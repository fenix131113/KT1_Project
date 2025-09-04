using GameAssembly.Game;
using GameAssembly.Services;
using UnityEngine;
using VContainer;

namespace GameAssembly.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float jumpForce = 2f;
        [SerializeField] private LayerMask deadByLayer;
        
        [Inject] private PlayerInput _playerInput;
        [Inject] private GameRestart _gameRestart;

        private void Start() => _playerInput.RegisterJumpHoldInputCallback(HoldJump);

        private void HoldJump()
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        private void Death() => _gameRestart.RestartGame();

        public void StartPlayer() => rb.bodyType = RigidbodyType2D.Dynamic;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, deadByLayer))
                return;
            
            Death();
        }
    }
}
