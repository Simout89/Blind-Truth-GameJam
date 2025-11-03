using System;
using System.Collections;
using FMODUnity;
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

    public event Action<AmmoInfo> OnAmmoChanged;
    
    private int TotalAmmo = 0;
    private int AmmoCountInClip = 0;
    private float _nextFireTime = 0f; // Время следующего возможного выстрела

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
        Reload();
    }

    private void HandleAttack(InputAction.CallbackContext obj)
    {
        if(!enable)
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
        if (AmmoCountInClip >= _characterController.CharacterControllerData.MaxAmmoInClip)
            return;

        if (TotalAmmo <= 0)
            return;

        int neededAmmo = _characterController.CharacterControllerData.MaxAmmoInClip - AmmoCountInClip;

        int ammoToLoad = Mathf.Min(neededAmmo, TotalAmmo);

        AmmoCountInClip += ammoToLoad;
        TotalAmmo -= ammoToLoad;
        RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_Reload");
        OnAmmoChanged?.Invoke(new AmmoInfo(TotalAmmo, AmmoCountInClip));
    }


    private void TryShoot()
    {
        if(AmmoCountInClip <= 0)
            return;
        
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

            var damageInfo = new DamageInfo(_characterController.CharacterControllerData.Damage, "player", transform);
            
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