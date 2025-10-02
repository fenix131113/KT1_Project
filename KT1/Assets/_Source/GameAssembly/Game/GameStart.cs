using GameAssembly.EnemySystem;
using GameAssembly.Level;
using GameAssembly.PlayerSystem;
using GameAssembly.ScoresSystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.Game
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameObject inputHint;
        [SerializeField] private MovingObject movingLayout;
        
        [Inject] private Player _player;
        [Inject] private PlayerInput _playerInput;
        [Inject] private EnemySpawner _enemySpawner;
        [Inject] private ScoresSpawner _scoresSpawner;

        private void Awake() => _playerInput.RegisterJumpHoldInputCallback(StartGame);

        private void StartGame()
        {
            _playerInput.UnregisterJumpHoldCallback(StartGame);
            
            inputHint.SetActive(false);
            _player.StartPlayer();
            movingLayout.enabled = true;
            _enemySpawner.StartSpawn();
            _scoresSpawner.StartSpawn();
        }
    }
}