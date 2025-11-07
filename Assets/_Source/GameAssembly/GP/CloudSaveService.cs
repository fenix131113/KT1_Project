using System;
using System.Collections;
using GameAssembly.Services;
using GamePush;
using UnityEngine;
using UnityEngine.Networking;

namespace GameAssembly.GP
{
    public static class CloudSaveService
    {
        public const string SCORE_KEY = "scores";

        public static void SaveData(string key, int data)
        {
            GP_Player.Set(key, data);
            GP_Player.Sync(SyncStorageType.local);
            GP_Player.Sync(SyncStorageType.cloud);
        }

        public static void LoadData(Action<int> setter)
        {
            CoroutineRunner.Instance.RunCoroutine(CheckConnection(AfterConnectionCheck, setter));
        }

        private static void AfterConnectionCheck(bool result, Action<int> callback)
        {
            var scores = 0;

            if (result)
                GP_Player.GetInt(SCORE_KEY);
            else if (PlayerPrefs.HasKey(SCORE_KEY))
                scores = PlayerPrefs.GetInt(SCORE_KEY);

            callback.Invoke(scores);
        }

        public static IEnumerator CheckConnection(Action<bool, Action<int>> callback, Action<int> setter)
        {
            using var request = UnityWebRequest.Get("https://www.google.com/generate_204");
            request.timeout = 5;
            yield return request.SendWebRequest();

            var ok = request.result == UnityWebRequest.Result.Success;
            callback?.Invoke(ok, setter);
        }
    }
}