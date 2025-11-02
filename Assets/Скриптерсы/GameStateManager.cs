using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы
    {
        public class GameStateManager: MonoBehaviour
        {
            [Inject] private CameraController _cameraController;
            [Inject] private PlayerInteraction _playerInteraction;
            public GameStates CurrentState => currentState;
            private GameStates currentState = GameStates.Play;
            public GameStates PreviousState => previousState;
            private GameStates previousState;
            public event Action<GameStates> OnStateChanged;

            private void Awake()
            {
                ChangeState(GameStates.Play);
                Cursor.lockState = CursorLockMode.Locked;

            }

            public void ChangeState(GameStates newState)
            {
                if(newState == currentState)
                    return;

                switch (newState)
                {
                    case GameStates.Play:
                    {
                        Time.timeScale = 1;
                        Cursor.lockState = CursorLockMode.Locked;
                        _cameraController.Enable();
                        _playerInteraction.Enable();
                    } break;
                    case GameStates.Pause:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                        _cameraController.Disable();
                        _playerInteraction.Disable();
                    } break;
                    case GameStates.Note:
                    {
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                        _cameraController.Disable();
                        _playerInteraction.Disable();
                    } break;
                }

                previousState = currentState;
                currentState = newState;
                
                OnStateChanged?.Invoke(currentState);
            }
        }

        public enum GameStates
        {
            Play,
            Pause,
            Note
        }
    }