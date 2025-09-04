using TMPro;
using UnityEngine;
using VContainer;

namespace GameAssembly.ScoresSystem
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        [Inject] private Scores _scores;

        private void Awake()
        {
            Bind();
            Redraw();
        }

        private void OnDestroy() => Expose();

        private void Redraw() => scoreText.text = _scores.ScoresCount.ToString();

        private void Bind() => _scores.OnScoresChanged += Redraw;

        private void Expose() => _scores.OnScoresChanged -= Redraw;
    }
}