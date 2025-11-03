using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class QuickTimeEvent: MonoBehaviour
    {
        [SerializeField] private float timeOnQTE = 5f;  
        [SerializeField] private float value = 5;
        private float currentValue = 0;
        [Inject] private IInputService _inputService;
        [Inject] private GameStateManager gameStateManager;

        public event Action OnStartQte;
        public event Action OnStopQte;
        public event Action OnValueChanged;

        private Coroutine qteFail;
        private Coroutine qte;

        private void OnEnable()
        {
            _inputService.InputSystemActions.Player.Attack.performed += HandlePerformed;
        }

        private void OnDisable()
        {
            _inputService.InputSystemActions.Player.Attack.performed -= HandlePerformed;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            currentValue += 1;
            OnValueChanged?.Invoke();
        }

        public void StartQTE()
        {
            if(qte == null && qteFail == null)
            {
                qte = StartCoroutine(QTE());
                qteFail = StartCoroutine(QTEFailTimer());
                gameStateManager.ChangeState(GameStates.QTE);
            }
        }

        public void StopQTE()
        {
            
        }

        private IEnumerator QTE()
        {
            OnStartQte?.Invoke();
            
            currentValue = 0;

            while (currentValue < value)
            {
                currentValue = Mathf.Max(0, currentValue - Time.unscaledDeltaTime);
                OnValueChanged?.Invoke();
                yield return null;
            }
            OnStopQte?.Invoke();
            gameStateManager.ChangeState(GameStates.Play);
            
            if(qteFail != null)
            {
                StopCoroutine(qteFail);
                qteFail = null;
            }
            qte = null;
        }

        private IEnumerator QTEFailTimer()
        {
            float elapsed = 0f;
            
            while (elapsed < timeOnQTE)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            
            OnStopQte?.Invoke();
            gameStateManager.ChangeState(GameStates.Play);

            if(qte != null)
            {
                StopCoroutine(qte);
                qte = null;
            }
            qteFail = null;
        }
    }
}