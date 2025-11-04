using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы.View
{
    public class PauseView: MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private SettingService _settingService;
        [SerializeField] private Slider _slider;

        [SerializeField] private GameObject[] pages;

        private void Awake()
        {
            Hide();
            _slider.value = _settingService.MouseSensitivity;
        }


        public void ChangedSensitivity(float value)
        {
            _settingService.ChangeMouseSensitivity(_slider.value);
        }

        public void Open()
        {
            pauseMenu.SetActive(true);
        }

        public void Hide()
        {
            pauseMenu.SetActive(false);

            foreach (var VARIABLE in pages)
            {
                VARIABLE.SetActive(false);
            }
            
            pages[0].SetActive(true);
        }

        public void Exit()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}