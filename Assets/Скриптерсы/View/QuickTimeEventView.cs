using System;
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
        }

        private void HandleStop()
        {
            qteGameObject.SetActive(false);
        }

        private void HandleValueChanged()
        {
            imageProgress.fillAmount = _quickTimeEvent.currentValue / _quickTimeEvent.QuickTimeEventData.MaxValue;
        }
    }
}