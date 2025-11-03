using UnityEngine;
using Zenject;

namespace Скриптерсы.Interactable
{
    public class WeaponPickUp: HighlightableObject, IClickable
    {
        [Inject] private WeaponCombatController _weaponCombatController;
        public ClickResult Click()
        {
            _weaponCombatController.PickUpWeapon();
            
            Destroy(gameObject);   

            return new ClickResult(true);
        }
    }
}