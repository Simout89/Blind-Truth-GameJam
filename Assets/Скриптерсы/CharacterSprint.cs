using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class CharacterSprint: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        private EnduranceSystem _enduranceSystem;
        private bool sprint = false;
        
        private void Awake()
        {
            _enduranceSystem = new EnduranceSystem(_characterController.CharacterControllerData.MaxValue, 0, _characterController.CharacterControllerData.RateOfDecrease, _characterController.CharacterControllerData.RateOfIncrease);
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_HardBreath");
        }

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
            var stat = _characterController.PlayerStats.MoveSpeed;
            if(!stat.TryFindAdditional("crouch") && _enduranceSystem.CurrentEndurance > 0)
            {
                sprint = true;
                _characterController.PlayerStats.MoveSpeed.AddAdditional("sprint", _characterController.CharacterControllerData.SprintMultiplayer);
            }
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            _characterController.PlayerStats.MoveSpeed.RemoveAdditional("sprint");
            sprint = false;
        }

        private void Update()
        {
            if (sprint)
            {
                _enduranceSystem.ReduceEndurance();
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Stamina", _enduranceSystem.CurrentEndurance);
                
                if (_enduranceSystem.CurrentEndurance <= 0)
                {
                    _characterController.PlayerStats.MoveSpeed.RemoveAdditional("sprint");
                    sprint = false;
                }
            }
            else
            {
                _enduranceSystem.AddEndurance();
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Stamina", _enduranceSystem.CurrentEndurance);

            }
        }
    }
}