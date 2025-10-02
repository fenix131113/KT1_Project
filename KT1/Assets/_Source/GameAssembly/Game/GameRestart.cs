using UnityEngine.SceneManagement;

namespace GameAssembly.Game
{
    public class GameRestart
    {
        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}