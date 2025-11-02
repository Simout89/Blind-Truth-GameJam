using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Скриптерсы;
using CharacterController = Скриптерсы.CharacterController;

public class WeaponCombatController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera camera;
    [SerializeField] private VisualEffect muzzleVfx;
    [SerializeField] private Light _light;
    [SerializeField] private float lightDuration = 0.1f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float fireRate = 0.5f;
    
    private float TotalAmmo = 0f;
    private float AmmoCountInClip = 0f;
    private float _nextFireTime = 0f; // Время следующего возможного выстрела

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
        if (Time.time < _nextFireTime)
        {
            return;
        }
        
        _nextFireTime = Time.time + _characterController.CharacterControllerData.DelayBetweenShots;
        
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = camera.ScreenPointToRay(screenCenter);
        
        muzzleVfx.Play();
        StartCoroutine(FlashLight());
        _animator.SetTrigger("Shoot");

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask, QueryTriggerInteraction.Ignore) && hit.collider.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log(hit.collider.name);

            var damageInfo = new DamageInfo(_characterController.CharacterControllerData.Damage, "player");
            
            damageable.TakeDamage(damageInfo);
        }
    }

    private IEnumerator FlashLight()
    {
        _light.enabled = true;
        
        // Ждём указанное количество секунд
        yield return new WaitForSeconds(lightDuration);
        
        _light.enabled = false;
    }
}