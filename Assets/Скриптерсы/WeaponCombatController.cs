using System;
using System.Collections;
using FMODUnity;
using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Zenject;
using Скриптерсы;
using Скриптерсы.Datas;
using CharacterController = Скриптерсы.CharacterController;

public class WeaponCombatController : MonoBehaviour
{
    [SerializeField] private WeaponFeedBackData  _weaponFeedBackData;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera camera;
    [SerializeField] private VisualEffect muzzleVfx;
    [SerializeField] private Light _light;
    [SerializeField] private float lightDuration = 0.1f;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] hands;
    
    [Inject] private CameraController _cameraController;

    private bool enable = true;
    private bool haveWeapon = false;
    private bool isReloading = false;

    public event Action<AmmoInfo> OnAmmoChanged;
    public event Action OnWeaponPickUp;
    
    private int TotalAmmo = 0;
    private int AmmoCountInClip = 0;
    private float _nextFireTime = 0f;

    private void OnEnable()
    {
        _characterController._inputService.InputSystemActions.Player.Attack.performed += HandleAttack;
        _characterController._inputService.InputSystemActions.Player.Reload.performed += HandleReload;

    }

    private void OnDisable()
    {
        _characterController._inputService.InputSystemActions.Player.Attack.performed -= HandleAttack;
        _characterController._inputService.InputSystemActions.Player.Reload.performed -= HandleReload;
    }

    private void HandleReload(InputAction.CallbackContext obj)
    {
        if(!enable)
            return;
        if(!haveWeapon)
            return;
        Reload();
    }

    private void HandleAttack(InputAction.CallbackContext obj)
    {
        if(!enable)
            return;
        if(!haveWeapon)
            return;
        
        TryShoot();
    }

    public void AddAmmo(int count)
    {
        TotalAmmo += count;
        OnAmmoChanged?.Invoke(new AmmoInfo(TotalAmmo, AmmoCountInClip));
    }

    public void Reload()
    {
        if(!haveWeapon)
            return;
        
        if (isReloading)
            return;
        
        if (AmmoCountInClip >= _characterController.CharacterControllerData.MaxAmmoInClip)
            return;

        if (TotalAmmo <= 0)
            return;
        
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        
        _animator.SetTrigger("Reload");
        RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_Reload");

        yield return new WaitForSeconds(_characterController.CharacterControllerData.ReloadTime);

        int neededAmmo = _characterController.CharacterControllerData.MaxAmmoInClip - AmmoCountInClip;
        int ammoToLoad = Mathf.Min(neededAmmo, TotalAmmo);

        AmmoCountInClip += ammoToLoad;
        TotalAmmo -= ammoToLoad;
        
        OnAmmoChanged?.Invoke(new AmmoInfo(TotalAmmo, AmmoCountInClip));
        
        isReloading = false;
    }


    private void TryShoot()
    {
        if(isReloading)
            return;
        
        if(AmmoCountInClip <= 0)
        {
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_NoAmmo");
            return;
        }
        
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
        RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_Fire");
        AmmoCountInClip--;
        OnAmmoChanged?.Invoke(new AmmoInfo(TotalAmmo, AmmoCountInClip));
        
        
        _cameraController.FovFade(_weaponFeedBackData.additionFov, _weaponFeedBackData.fadeInDuration, _weaponFeedBackData.fadeOutDuration);
        _cameraController.Shake(_weaponFeedBackData.tiltIntensity, UnityEngine.Random.Range(_weaponFeedBackData.leftRightTilt.x, _weaponFeedBackData.leftRightTilt.y), _weaponFeedBackData.duration);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask, QueryTriggerInteraction.Ignore) && hit.collider.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log(hit.collider.name);

            Vector3 directionToCamera = (camera.transform.position - hit.point).normalized;
            LeanPool.Despawn(LeanPool.Spawn(_characterController.CharacterControllerData.bloodVfx, hit.point,
                Quaternion.LookRotation(directionToCamera), null), 3f);

            var damageInfo = new DamageInfo(_characterController.CharacterControllerData.Damage, "player", transform);
            
            damageable.TakeDamage(damageInfo);
        }
    }

    private IEnumerator FlashLight()
    {
        _light.enabled = true;
        
        yield return new WaitForSeconds(lightDuration);
        
        _light.enabled = false;
    }

    public void ShowHands()
    {
        foreach (var VARIABLE in hands)
        {
            VARIABLE.SetActive(true);
        }
    }

    public void HideHands()
    {
        foreach (var VARIABLE in hands)
        {
            VARIABLE.SetActive(false);
        }
    }

    public void Enable()
    {
        enable = true;
    }

    public void Disable()
    {
        enable = false;
    }

    public void PickUpWeapon()
    {
        haveWeapon = true;
        _animator.SetTrigger("PickUp");
        OnWeaponPickUp?.Invoke();
    }
}

public struct AmmoInfo
{
    public int TotalAmmo;
    public int AmmoCountInClip;
    public AmmoInfo(int TotalAmmo, int AmmoCountInClip)
    {
        this.TotalAmmo = TotalAmmo;
        this.AmmoCountInClip = AmmoCountInClip;
    }
}