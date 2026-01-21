using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Скриптерсы
{
    public class PlayerCrouch: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform body;
        [SerializeField] private float crouchScale = 0.5f;
        [SerializeField] private float crouchDuration = 0.2f;
        [SerializeField] private float overheadCheckDistance = 1f;
        [SerializeField] private float overheadCheckRadius = 0.3f;
        [SerializeField] private LayerMask obstacleMask;

        private float _originalScaleY;
        private Coroutine _crouchCoroutine;
        private bool _isCrouching;
        private bool _wantsToStand; // Флаг: игрок хочет встать

        private void Awake()
        {
            if (body != null)
            {
                _originalScaleY = body.localScale.y;
            }
        }

        private void OnEnable()
        {
            _characterController._inputService.InputSystemActions.Player.Crouch.performed += HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Crouch.canceled += HandleCanceled;
        }

        private void OnDisable()
        {
            _characterController._inputService.InputSystemActions.Player.Crouch.performed -= HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Crouch.canceled -= HandleCanceled;
        }

        private void Update()
        {
            // Постоянная проверка: если игрок хочет встать и теперь может - встаем
            if (_wantsToStand && _isCrouching && CanStandUp())
            {
                StandUp();
            }
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            var stat = _characterController.PlayerStats.MoveSpeed;
            if(!stat.TryFindAdditional("sprint"))
            {
                stat.AddAdditional("crouch", _characterController.CharacterControllerData.CrouchMultiplayer);
                _isCrouching = true;
                _wantsToStand = false; // Сбрасываем желание встать
                
                if (_crouchCoroutine != null)
                {
                    StopCoroutine(_crouchCoroutine);
                }
                _crouchCoroutine = StartCoroutine(ScaleBodyCoroutine(_originalScaleY * crouchScale));
            }
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            _wantsToStand = true; // Помечаем, что игрок хочет встать
            
            if (CanStandUp())
            {
                StandUp();
            }
            // Если не можем встать - ничего не делаем, Update() будет проверять
        }

        private void StandUp()
        {
            _characterController.PlayerStats.MoveSpeed.RemoveAdditional("crouch");
            _isCrouching = false;
            _wantsToStand = false;
            
            if (_crouchCoroutine != null)
            {
                StopCoroutine(_crouchCoroutine);
            }
            _crouchCoroutine = StartCoroutine(ScaleBodyCoroutine(_originalScaleY));
        }

        private bool CanStandUp()
        {
            Vector3 checkPosition = body.position + Vector3.up * overheadCheckDistance;
            return !Physics.CheckSphere(checkPosition, overheadCheckRadius, obstacleMask, QueryTriggerInteraction.Ignore);
        }

        private IEnumerator ScaleBodyCoroutine(float targetScaleY)
        {
            if (body == null) yield break;

            float startScaleY = body.localScale.y;
            float elapsed = 0f;

            while (elapsed < crouchDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / crouchDuration;
                float newScaleY = Mathf.Lerp(startScaleY, targetScaleY, t);
                body.localScale = new Vector3(body.localScale.x, newScaleY, body.localScale.z);
                yield return null;
            }

            body.localScale = new Vector3(body.localScale.x, targetScaleY, body.localScale.z);
            _crouchCoroutine = null;
        }

        private void OnDrawGizmos()
        {
            if (body == null) return;

            Vector3 checkPosition = body.position + Vector3.up * overheadCheckDistance;
            
            // Красная сфера если есть препятствие, зеленая если свободно
            Gizmos.color = CanStandUp() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(checkPosition, overheadCheckRadius);
        }
    }
}