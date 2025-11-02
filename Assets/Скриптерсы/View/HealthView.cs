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

        private void HandleChanged()
        {
            DamageEffect();
        }
        
        private void DamageEffect()
        {
            _canvas.DOComplete();

            _canvas.alpha = 1;
            
            _canvas.DOFade(0, 1);
        }
    }
}