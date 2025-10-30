using System;
using GameAnalyticsSDK;
using UnityEngine;

namespace GameAssembly.Analytics
{
    public class GAInit : MonoBehaviour
    {
        private void Awake()
        {
            GameAnalytics.Initialize();
        }
    }
}
