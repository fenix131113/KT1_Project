using System.Collections;
using GameAssembly.PlayerSystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.ScoresSystem
{
    public class ScoresSpawner : MonoBehaviour
    {
        [SerializeField] private ScoreCollectable scorePrefab;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [SerializeField] private float minTimeBetweenSpawn;
        [SerializeField] private float maxTimeBetweenSpawn;
        [SerializeField] private float spawnDistanceFromPlayer;

        [Inject] private Player _player;
        [Inject] private Scores _scores;

        public void StartSpawn() => StartCoroutine(SpawnRoutine());

        private void SpawnScore()
        {
            Instantiate(scorePrefab,
                new Vector3(_player.transform.position.x + spawnDistanceFromPlayer, Random.Range(minY, maxY), 0),
                Quaternion.identity).Init(_scores);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn));

                SpawnScore();
            }
        }
    }
}