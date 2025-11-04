using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class InputHandler: MonoBehaviour
    {
        [Inject] private IInputService _inputService;

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
            
        }
    }
}