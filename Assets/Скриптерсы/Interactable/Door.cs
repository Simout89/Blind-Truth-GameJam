using UnityEngine;
using DG.Tweening;
using FMODUnity;

namespace Скриптерсы.Interactable
{
    public class Door : MonoBehaviour
    {
        [Header("Поворот двери")]
        [SerializeField] private Vector3 rotationOffset;
        [Header("Смещение двери")]
        [SerializeField] private Vector3 positionOffset;
        [Header("Настройки")]
        [SerializeField] private float openDuration = 1f; // скорость открытия
        [SerializeField] private float closeDuration = 1f; // скорость закрытия
        [SerializeField] private bool useRotation = true; // включить поворот
        [SerializeField] private bool usePosition = false; // включить движение
        [SerializeField] private string OpenSoundEvent;
        [SerializeField] private string CloseSoundEvent;

        private Vector3 initialPosition;
        private Vector3 initialRotation;
        private bool isOpen = false;

        private void Awake()
        {
            initialPosition = transform.position;
            initialRotation = transform.eulerAngles;
        }

        public void Open()
        {
            if (!isOpen)
            {
                if (useRotation)
                    transform.DORotate(initialRotation + rotationOffset, openDuration).SetEase(Ease.InOutSine);
                if (usePosition)
                    transform.DOMove(initialPosition + positionOffset, openDuration).SetEase(Ease.InOutSine);
                if(OpenSoundEvent != "")
                    RuntimeManager.PlayOneShot(OpenSoundEvent);

                isOpen = true;
            }
        }

        public void Close()
        {
            if (isOpen)
            {
                if (useRotation)
                    transform.DORotate(initialRotation, closeDuration).SetEase(Ease.InOutSine);
                if (usePosition)
                    transform.DOMove(initialPosition, closeDuration).SetEase(Ease.InOutSine);
                if(CloseSoundEvent != "")
                    RuntimeManager.PlayOneShot(CloseSoundEvent);

                isOpen = false;
            }
        }

        public void Toggle()
        {
            if (isOpen)
                Close();
            else
                Open();
        }
    }
}