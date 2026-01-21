using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Скриптерсы.View
{
    public class HealthView: MonoBehaviour
    {
        [Inject] private PlayerHealth _playerHealth;
        [SerializeField] private CanvasGroup _canvas;

        private void OnEnable()
        {
            _playerHealth.OnHealthChanged += HandleChanged;
        }

        private void OnDisable()
        {
            _playerHealth.OnHealthChanged -= HandleChanged;
        }
        
        private void Awake()
        {
            _canvas.alpha = 0;
        }

        private void HandleChanged(float n)
        {

            if (n < 0)
            {
                DamageEffect();
            }
            else
            {
                _canvas.DOFade(0, 2);
            }
        }
        
        private void DamageEffect()
        {
            _canvas.DOComplete();

            _canvas.alpha = 1;

            if (_playerHealth.CurrentHealth >= _playerHealth._characterController.CharacterControllerData.Health / 3)
            {
                _canvas.DOFade(0, 2);
            }
        }
    }
}