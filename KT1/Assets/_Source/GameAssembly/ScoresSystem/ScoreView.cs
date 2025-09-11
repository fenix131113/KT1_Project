using TMPro;
using UnityEngine;
using VContainer;

namespace GameAssembly.ScoresSystem
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        [Inject] private Score _score;

        private void Awake()
        {
            Bind();
            Redraw();
        }

        private void OnDestroy() => Expose();

        private void Redraw() => scoreText.text = _score.ScoresCount.ToString();

        private void Bind() => _score.OnScoresChanged += Redraw;

        private void Expose() => _score.OnScoresChanged -= Redraw;
    }
}