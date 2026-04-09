using System.Collections;
using GameAssembly.Core;
using GameAssembly.Level.Data;
using GameAssembly.ReplaySystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private SpawnSettings spawnSettings;

        [Inject] private IPositionGetter _playerPosition;
        [Inject] private IRng _rng;
        [Inject] private ReplayController _replayController;

        private bool _isSpawning;

        private void Start() => _replayController.OnPlaybackCommand += HandlePlaybackCommand;

        private void OnDestroy()
        {
            if (_replayController != null)
                _replayController.OnPlaybackCommand -= HandlePlaybackCommand;
        }

        public void StartSpawn()
        {
            if (_replayController.IsPlayback || _isSpawning)
                return;

            _isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }

        private void SpawnEnemy(float x, float y)
        {
            Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_rng.Range(spawnSettings.MinTimeBetweenSpawn, spawnSettings.MaxTimeBetweenSpawn));

                var spawnX = _playerPosition.GetPosition().x + spawnSettings.SpawnDistanceFromPlayer;
                var spawnY = _rng.Range(spawnSettings.MinY, spawnSettings.MaxY);

                SpawnEnemy(spawnX, spawnY);
                _replayController.RecordCommand(ReplayCommand.EnemySpawn(_replayController.CurrentTick, spawnX, spawnY));
            }
        }

        private void HandlePlaybackCommand(ReplayCommand command)
        {
            if (command.type != ReplayCommandType.ENEMY_SPAWN)
                return;

            SpawnEnemy(command.floatValue, command.floatValue2);
        }
    }
}
