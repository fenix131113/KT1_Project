using System.Collections;
using UnityEngine;

namespace GameAssembly.Services
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (!_instance)
                    _instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();

                return _instance;
            }
        }

        public void RunCoroutine(IEnumerator routine) => StartCoroutine(routine);
    }
}