using GameAssembly.Core;
using GameAssembly.Core.Data;
using GameAssembly.Game;
using GameAssembly.Services;
using UnityEngine;
using VContainer;

namespace GameAssembly.PlayerSystem
{
    public class Player : MonoBehaviour, IPositionGetter
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float jumpForce = 2f;
        
        [Inject] private PlayerInput _playerInput;
        [Inject] private GameRestart _gameRestart;
        [Inject] private LayersDataSO _layersDataSO;

        private void Start() => _playerInput.RegisterJumpHoldInputCallback(HoldJump);

        public Vector3 GetPosition() => transform.position;

        private void HoldJump()
        {
            rb.AddForce(Vector2.up * (jumpForce * Time.deltaTime), ForceMode2D.Force);
        }

        private void Death() => _gameRestart.RestartGame();

        public void StartPlayer() => rb.bodyType = RigidbodyType2D.Dynamic;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, _layersDataSO.PlayerDeadLayer))
                return;
            
            Death();
        }
    }
}
