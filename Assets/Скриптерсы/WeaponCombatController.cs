using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Скриптерсы;
using CharacterController = Скриптерсы.CharacterController;

public class WeaponCombatController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    
    [SerializeField] private Camera camera;

    private void OnEnable()
    {
        _characterController._inputService.InputSystemActions.Player.Attack.performed += HandleAttack;
    }

    private void OnDisable()
    {
        _characterController._inputService.InputSystemActions.Player.Attack.performed -= HandleAttack;
    }

    private void HandleAttack(InputAction.CallbackContext obj)
    {
        TryShoot();
    }

    private void TryShoot()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = camera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, 100) && hit.collider.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log(hit.collider.name);

            var damageInfo = new DamageInfo(_characterController.CharacterControllerData.Damage, "player");
            
            damageable.TakeDamage(damageInfo);
        }
    }
}
