using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Скриптерсы
{
    public class QuickTimeEventView: MonoBehaviour
    {
        [Inject] private QuickTimeEvent _quickTimeEvent;
        [SerializeField] private GameObject qteGameObject;
        [SerializeField] private Image imageProgress;
        [SerializeField] private CanvasGroup rightEye;
        [SerializeField] private CanvasGroup leftEye;
        [SerializeField] private Animator _animator;
        
        
        private void OnEnable()
        {
            _quickTimeEvent.OnStartQte += HandleStart;
            _quickTimeEvent.OnStopQte += HandleStop;
            _quickTimeEvent.OnValueChanged += HandleValueChanged;
        }
        
        private void OnDisable()
        {
            _quickTimeEvent.OnStartQte -= HandleStart;
            _quickTimeEvent.OnStopQte -= HandleStop;
            _quickTimeEvent.OnValueChanged -= HandleValueChanged;
        }

        private void Awake()
        {
            HandleStop();
        }

        private void HandleStart()
        {
            qteGameObject.SetActive(true);
            // Сбрасываем все триггеры перед установкой нового
            _animator.ResetTrigger("Stop");
            _animator.ResetTrigger("PlayLeftEye");
            _animator.SetTrigger("PlayRightEye");
        }

        private void HandleStop()
        {
            qteGameObject.SetActive(false);
            // Сбрасываем все активные триггеры перед остановкой
            _animator.ResetTrigger("PlayRightEye");
            _animator.ResetTrigger("PlayLeftEye");
            _animator.SetTrigger("Stop");
        }

        private void HandleValueChanged()
        {
            imageProgress.fillAmount = _quickTimeEvent.currentValue / _quickTimeEvent.QuickTimeEventData.MaxValue;

            if (_quickTimeEvent.currentValue >= _quickTimeEvent.QuickTimeEventData.MaxValue / 2)
            {
                _animator.ResetTrigger("PlayRightEye");
                _animator.SetTrigger("PlayLeftEye");
            }
        }

        public void BlinkEye(Eyes eyes)
        {
            if (eyes == Eyes.Right)
            {
                rightEye.alpha = 1;
                rightEye.DOComplete();
                rightEye.DOFade(0, 0.2f);
            }
            else
            {
                leftEye.alpha = 1;
                leftEye.DOComplete();
                leftEye.DOFade(0, 0.2f);
            }
        }
    }

    public enum Eyes
    {
        Left,
        Right
    }
}