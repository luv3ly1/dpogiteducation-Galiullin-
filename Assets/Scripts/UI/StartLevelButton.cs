using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartLevelButton : MonoBehaviour
    {
        [SerializeField] private Button mainButton;
        [SerializeField] private int levelNumber;

        private void OnValidate()
        {
            mainButton = GetComponent<Button>();
        }

        private void Awake()
        {
            mainButton.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            AllLevelsData.LoadLevel(levelNumber);
        }
    }
}