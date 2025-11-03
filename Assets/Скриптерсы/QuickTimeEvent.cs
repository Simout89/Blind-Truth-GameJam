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
            value += 1;
        }

        public void StartQTE()
        {
            qte = StartCoroutine(QTE());
            qteFail = StartCoroutine(QTEFailTimer());
        }

        public void StopQTE()
        {
            
        }

        private IEnumerator QTE()
        {
            currentValue = 0;

            while (currentValue < value)
            {
                currentValue = Mathf.Max(0, currentValue - Time.deltaTime);
                yield return null;
            }
            
            StopCoroutine(qteFail);
        }

        private IEnumerator QTEFailTimer()
        {
            yield return new WaitForSeconds(timeOnQTE);
            StopCoroutine(qte);
            
        }
    }
}