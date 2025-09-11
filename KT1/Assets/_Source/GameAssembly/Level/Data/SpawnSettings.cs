using UnityEngine;

namespace GameAssembly.Level.Data
{
    [CreateAssetMenu(fileName = "SpawnSettings", menuName = "ScriptableObjects/SpawnSettings")]
    public class SpawnSettings : ScriptableObject
    {
        [field: SerializeField] public float MinY { get; private set; }
        [field: SerializeField] public float MaxY{ get; private set; }
        [field: SerializeField] public float MinTimeBetweenSpawn{ get; private set; }
        [field: SerializeField] public float MaxTimeBetweenSpawn{ get; private set; }
        [field: SerializeField] public float SpawnDistanceFromPlayer{ get; private set; }
    }
}