using UnityEngine;

namespace GameAssembly.Level
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
    
        private void Update() => transform.position += Vector3.right * (moveSpeed * Time.deltaTime);
    }
}
