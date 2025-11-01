using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class CharacterSprint: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        private void OnEnable()
        {
            _characterController._inputService.InputSystemActions.Player.Sprint.performed += HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Sprint.canceled += HandleCanceled;
        }

        private void OnDisable()
        {
            _characterController._inputService.InputSystemActions.Player.Sprint.performed -= HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Sprint.canceled -= HandleCanceled;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            _characterController.PlayerStats.MoveSpeed.AddAdditional("sprint", _characterController.CharacterControllerData.SprintMultiplayer);
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            _characterController.PlayerStats.MoveSpeed.RemoveAdditional("sprint");
        }
    }
}