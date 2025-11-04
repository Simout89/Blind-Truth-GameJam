using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы
{
    public class SceneEntryPoint: MonoBehaviour
    {
        [Inject] private CharacterController _characterController;
        [Inject] private CameraController _cameraController;
        [Inject] private WeaponCombatController _weaponCombatController;
        [Inject] private SaveRepository _saveRepository;
        
        public void Awake()
        {
            if(_saveRepository.PlayerSave is null)
                return;
            
            _characterController.Teleport(_saveRepository.PlayerSave.position);
            _cameraController.SetPanTilt(_saveRepository.PlayerSave.panTilt);
            if (_saveRepository.PlayerSave.haveWeapon)
            {
                _weaponCombatController.PickUpWeapon();
            }
            _weaponCombatController.AddAmmo(_saveRepository.PlayerSave.ammo);
        }
    }
}