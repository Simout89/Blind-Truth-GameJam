using UnityEngine;
using Zenject;

namespace Скриптерсы
{
    public class CheckPointManager: MonoBehaviour
    {
        [Inject] private SaveRepository saveRepository;
        [Inject] private CharacterController _characterController;
        [Inject] private CameraController _cameraController;
        [Inject] private WeaponCombatController _weaponCombatController;
        
        public void Save()
        {
            saveRepository.PlayerSave = new PlayerSave(_characterController.transform.position, _cameraController.CinemachinePanTilt.PanAxis.Value, _weaponCombatController.GetAmmo(), _weaponCombatController.HaveWeapon);
        }
    }
}