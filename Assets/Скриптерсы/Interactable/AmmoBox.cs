using UnityEngine;
using Zenject;
using Скриптерсы.Datas;

namespace Скриптерсы.Interactable
{
    public class AmmoBox: HighlightableObject, IClickable
    {
        [SerializeField] private AmmoBoxData _ammoBoxData;
        [Inject] private WeaponCombatController _weaponCombatController;
        
        public ClickResult Click()
        {
            _weaponCombatController.AddAmmo(_ammoBoxData.AmmoCount);
            
            Destroy(gameObject);   

            return new ClickResult(true);
        }
    }
}