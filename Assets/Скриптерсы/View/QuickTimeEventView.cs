using System;
using DG.Tweening;
using FMODUnity;
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
        [SerializeField] private GameObject rightBlack;
        [SerializeField] private GameObject leftBlack;
        [SerializeField] private Animator _animator;

        private bool enable = false;
        
        
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
            // Деактивируем черные объекты при запуске
            rightBlack.SetActive(false);
            leftBlack.SetActive(false);
        }

        private void HandleStart()
        {
            enable = true;
            
            qteGameObject.SetActive(true);
            // Сбрасываем черные объекты при старте нового QTE
            rightBlack.SetActive(false);
            leftBlack.SetActive(false);
            // Сбрасываем все триггеры перед установкой нового
            _animator.ResetTrigger("Stop");
            _animator.ResetTrigger("PlayLeftEye");
            _animator.SetTrigger("PlayRightEye");
        }

        private void HandleStop()
        {
            enable = false;
            
            qteGameObject.SetActive(false);
            // Сбрасываем все активные триггеры перед остановкой
            _animator.ResetTrigger("PlayRightEye");
            _animator.ResetTrigger("PlayLeftEye");
            _animator.SetTrigger("Stop");
            // Деактивируем оба черных объекта при остановке
            rightBlack.SetActive(false);
            leftBlack.SetActive(false);
        }

        private void HandleValueChanged()
        {
            if(!enable)
                return;
            
            imageProgress.fillAmount = _quickTimeEvent.currentValue / _quickTimeEvent.QuickTimeEventData.MaxValue;

            if (_quickTimeEvent.currentValue >= _quickTimeEvent.QuickTimeEventData.MaxValue / 2)
            {
                _animator.ResetTrigger("PlayRightEye");
                _animator.SetTrigger("PlayLeftEye");
                rightBlack.SetActive(true);
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
            
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_Stab");

        }
    }

    public enum Eyes
    {
        Left,
        Right
    }
}