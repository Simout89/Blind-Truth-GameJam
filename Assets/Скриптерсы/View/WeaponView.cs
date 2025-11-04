using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Скриптерсы.View
{
    public class WeaponView: MonoBehaviour
    {
        [Inject] private WeaponCombatController _weaponCombatController;
        [SerializeField] private TMP_Text _tmpText;

        private void OnEnable()
        {
            _weaponCombatController.OnAmmoChanged += HandleAmmoChanged;
            _weaponCombatController.OnWeaponPickUp += HandlePickUp;
        }

        private void OnDisable()
        {
            _weaponCombatController.OnAmmoChanged -= HandleAmmoChanged;
            _weaponCombatController.OnWeaponPickUp -= HandlePickUp;

        }

        private void HandlePickUp()
        {
            _tmpText.gameObject.SetActive(true);
        }

        private void Awake()
        {
            _tmpText.text = $"{0}/{0}";
            _tmpText.gameObject.SetActive(false);
        }

        private void HandleAmmoChanged(AmmoInfo obj)
        {
            _tmpText.text = $"{obj.AmmoCountInClip}/{obj.TotalAmmo}";
        }
    }
}