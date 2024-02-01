using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Button mainButton;
        [SerializeField] private Image line;
        [SerializeField] private TMP_Text text;

        private static bool IsSound = true;

        private void OnValidate()
        {
            mainButton = GetComponent<Button>();
        }

        private void Awake()
        {
            mainButton.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            if (IsSound)
                On();
            else 
                Off();
        }

        private void OnClick()
        {
            if (IsSound)
            {
                IsSound = false;
                Off();
            }
            else
            {
                IsSound = true;
                On();
            }
        }

        private void On()
        {
            AudioListener.pause = false;
            AudioListener.volume = 1;
                
            line.gameObject.SetActive(false);
            text.color = Color.black;
        }

        private void Off()
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
                
            line.gameObject.SetActive(true);
            text.color = Color.red;
        }
    }
}