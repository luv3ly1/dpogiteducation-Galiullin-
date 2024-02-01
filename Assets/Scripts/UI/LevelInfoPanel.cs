using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private void Start()
        {
            Level.Instance.scoreChanged.AddListener(OnScoreChanged);
            OnScoreChanged(0);
        }

        private void OnScoreChanged(int newScore)
        {
            scoreText.text = $"{newScore}/{Level.Instance.AllScore}";
        }
    }
}