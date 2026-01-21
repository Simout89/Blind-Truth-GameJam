using System;
using System.Collections;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Скриптерсы
{
    public class QuickTimeEventView: MonoBehaviour
    {
        [Inject] private QuickTimeEvent _quickTimeEvent;
        [Inject] private GameStateManager _gameStateManager;
        [SerializeField] private GameObject qteGameObject;
        [SerializeField] private Image imageProgress;
        [SerializeField] private CanvasGroup rightEye;
        [SerializeField] private CanvasGroup leftEye;
        [SerializeField] private GameObject rightBlack;
        [SerializeField] private GameObject leftBlack;
        [SerializeField] private GameObject blackScreen;
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [SerializeField] private CanvasGroup exitButton;
        [SerializeField] private GameObject mouseBlink;

        private bool enable = false;


        private EventInstance _eventInstance;


        private Coroutine _coroutine;
        
        
        private void OnEnable()
        {
            _quickTimeEvent.OnStartQte += HandleStart;
            _quickTimeEvent.OnStopQte += HandleStop;
            _quickTimeEvent.OnValueChanged += HandleValueChanged;

            _quickTimeEvent.OnDone += HandleDone;
        }
        
        private void OnDisable()
        {
            _quickTimeEvent.OnStartQte -= HandleStart;
            _quickTimeEvent.OnStopQte -= HandleStop;
            _quickTimeEvent.OnValueChanged -= HandleValueChanged;
            
            _quickTimeEvent.OnDone -= HandleDone;
        }

        private void HandleDone()
        {
            _gameStateManager.ChangeState(GameStates.End);
            blackScreen.SetActive(true);


            StopCoroutine(_coroutine);
            
            exitButton.DOFade(1, 8).SetDelay(5f).SetUpdate(true).SetEase(Ease.InOutSine);

        }
        
        IEnumerator ToggleActive()
        {
            while (true)
            {
                mouseBlink.SetActive(!mouseBlink.activeSelf);
                yield return new WaitForSeconds(0.3f);
            }
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
            _eventInstance = RuntimeManager.CreateInstance("event:/SFX/InGame/Player/p_Scream");
            _eventInstance.start();

            _coroutine = StartCoroutine(ToggleActive());
            
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
            
            _eventInstance.setParameterByName("Volume", 0);
            
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
            _impulseSource.GenerateImpulse(Random.insideUnitSphere);
        }
    }

    public enum Eyes
    {
        Left,
        Right
    }
}