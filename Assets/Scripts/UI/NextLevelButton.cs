using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private Button mainButton;
        [SerializeField] private bool isReload;

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
            if(!isReload)
                VKSDK.Instance.LevelCompleted();
            
            AllLevelsData.LoadLevel();
        }
    }
}