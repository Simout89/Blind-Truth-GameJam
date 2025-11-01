using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Скриптерсы
{
    public class PlayerCrouch: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        private void OnEnable()
        {
            _characterController._inputService.InputSystemActions.Player.Crouch.performed += HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Crouch.canceled += HandleCanceled;
        }

        private void OnDisable()
        {
            _characterController._inputService.InputSystemActions.Player.Crouch.performed -= HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Crouch.canceled -= HandleCanceled;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            var stat = _characterController.PlayerStats.MoveSpeed;
            if(!stat.TryFindAdditional("sprint"))
                stat.AddAdditional("crouch", _characterController.CharacterControllerData.CrouchMultiplayer);
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            _characterController.PlayerStats.MoveSpeed.RemoveAdditional("crouch");
        }
    }
}