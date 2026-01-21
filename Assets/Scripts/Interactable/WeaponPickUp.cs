using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Interactable
{
    public class WeaponPickUp: HighlightableObject, IClickable
    {
        [Inject] private WeaponCombatController _weaponCombatController;
        [Inject] private SaveRepository _saveRepository;
        [Inject] private CheckPointManager _checkPointManager;

        private void Start()
        {
            
            
            if(_saveRepository.PlayerSave != null && _saveRepository.PlayerSave.haveWeapon)
                Destroy(gameObject);
        }

        public ClickResult Click()
        {
            _weaponCombatController.PickUpWeapon();
            
            _checkPointManager.Save();
            
            Destroy(gameObject);   

            return new ClickResult(true);
        }
    }
}