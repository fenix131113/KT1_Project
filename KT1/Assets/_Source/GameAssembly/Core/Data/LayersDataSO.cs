using UnityEngine;

namespace GameAssembly.Core.Data
{
    [CreateAssetMenu(fileName = "LayersData", menuName = "ScriptableObjects/LayersData")]
    public class LayersDataSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
        [field: SerializeField] public LayerMask PlayerDeadLayer { get; private set; }
    }
}