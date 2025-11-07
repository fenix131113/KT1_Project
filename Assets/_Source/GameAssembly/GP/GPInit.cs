using GamePush;
using TMPro;
using UnityEngine;

namespace GameAssembly.GP
{
    public class GPInit : MonoBehaviour
    {
        [SerializeField] private TMP_Text idLabel;
        
        private async void Start()
        {
            if (!GP_Init.isReady)
            {
                idLabel.text = "Initializing...";
                await GP_Init.Ready;
            }
            
            GP_InitOnOnReady();
        }

        private void OnEnable()
        {
            GP_Init.OnReady += GP_InitOnOnReady;
            GP_Player.OnLoginComplete += GP_PlayerOnOnLoginComplete;
            GP_Player.OnLoginError += GP_PlayerOnOnLoginError;
        }

        private void OnDisable()
        {
            GP_Init.OnReady -= GP_InitOnOnReady;
            GP_Player.OnLoginComplete -= GP_PlayerOnOnLoginComplete;
            GP_Player.OnLoginError -= GP_PlayerOnOnLoginError;
        }
        
        private void GP_InitOnOnReady()
        {
            idLabel.text = "Logging...";
            GP_Player.Login();
        }

        private void GP_PlayerOnOnLoginError()
        {
            Debug.LogWarning("Login Error!");
        }

        private void GP_PlayerOnOnLoginComplete()
        {
            Debug.Log("Login Success!");
            var credentials = GP_Player.GetString("credentials");
            idLabel.text = string.IsNullOrEmpty(credentials) ? "ID: NaN" : credentials;
        }
    }
}
