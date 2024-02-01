using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] interfaces;

        private void Start()
        {
            interfaces[0].SetActive(Screen.height > Screen.width);
            interfaces[1].SetActive(Screen.height < Screen.width);
        }
    }
}
