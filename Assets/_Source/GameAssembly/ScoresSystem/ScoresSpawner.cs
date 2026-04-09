using System.Collections;
using GameAssembly.Core;
using GameAssembly.Core.Data;
using GameAssembly.Level.Data;
using GameAssembly.ReplaySystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.ScoresSystem
{
    public class ScoresSpawner : MonoBehaviour
    {
        [SerializeField] private ScoreCollectable scorePrefab;
        [SerializeField] private SpawnSettings spawnSettings;

        [Inject] private IPositionGetter _player;
        [Inject] private LayersDataSO _layersDataSO;
        [Inject] private Score _score;
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

        private void SpawnScore(float x, float y)
        {
            Instantiate(scorePrefab, new Vector3(x, y, 0), Quaternion.identity)
                .Init(_score, _layersDataSO);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_rng.Range(spawnSettings.MinTimeBetweenSpawn, spawnSettings.MaxTimeBetweenSpawn));

                var spawnX = _player.GetPosition().x + spawnSettings.SpawnDistanceFromPlayer;
                var spawnY = _rng.Range(spawnSettings.MinY, spawnSettings.MaxY);

                SpawnScore(spawnX, spawnY);
                _replayController.RecordCommand(ReplayCommand.ScoreSpawn(_replayController.CurrentTick, spawnX, spawnY));
            }
        }

        private void HandlePlaybackCommand(ReplayCommand command)
        {
            if (command.type != ReplayCommandType.SCORE_SPAWN)
                return;

            SpawnScore(command.floatValue, command.floatValue2);
        }
    }
}
