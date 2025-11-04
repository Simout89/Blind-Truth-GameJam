using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;
using Скриптерсы.View;

namespace Скриптерсы
{
    public class InputHandler: MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        [Inject] private PauseView _pauseView;
        [Inject] private GameStateManager _gameStateManager;

        private bool isPause = false;

        private void OnEnable()
        {
            _inputService.InputSystemActions.Player.Menu.performed += HandleMenu;
        }

        private void OnDisable()
        {
            _inputService.InputSystemActions.Player.Menu.performed -= HandleMenu;
        }

        private void HandleMenu(InputAction.CallbackContext obj)
        {
            isPause = !isPause;
            
            if (isPause)
            {
                _pauseView.Open();
                _gameStateManager.ChangeState(GameStates.Pause);
            }
            else
            {
                _pauseView.Hide();
                _gameStateManager.ChangeState(_gameStateManager.PreviousState);

            }
        }

        public void Resume()
        {
            _pauseView.Hide();
            _gameStateManager.ChangeState(_gameStateManager.PreviousState);

            isPause = false;
        }
    }
}