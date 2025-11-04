using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Скриптерсы.View
{
    public class PauseView: MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [Inject] private GameStateManager _gameStateManager;

        private void Awake()
        {
            Hide();
        }

        public void Open()
        {
            pauseMenu.SetActive(true);
        }

        public void Hide()
        {
            pauseMenu.SetActive(false);
        }

        public void Exit()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}