using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class QuickTimeEvent: MonoBehaviour
    {
        [field: SerializeField] public QuickTimeEventData QuickTimeEventData { get; private set; }
        public float currentValue { get; private set; } = 0;
        [Inject] private IInputService _inputService;
        [Inject] private GameStateManager gameStateManager;

        public event Action OnStartQte;
        public event Action OnStopQte;
        public event Action OnValueChanged;
        public event Action OnDone;

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
            currentValue += QuickTimeEventData.RateOfIncrease;
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

            while (currentValue < QuickTimeEventData.MaxValue)
            {
                currentValue = Mathf.Max(0, currentValue - Time.unscaledDeltaTime * QuickTimeEventData.RateOfDecrease);
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
            OnDone?.Invoke();
            qte = null;
        }

        private IEnumerator QTEFailTimer()
        {
            float elapsed = 0f;
            
            while (elapsed < QuickTimeEventData.TimeOnQTE)
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