using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using Скриптерсы.Datas;
using Скриптерсы.Enemy;

namespace Скриптерсы
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private TriggerDetector _triggerDetector; 
        public event Action OnClickableEnter;
        public event Action OnClickableExit;

        private List<KeyItemData> _keyItemDatas = new List<KeyItemData>();
        public List<KeyItemData> KeyItemDatas => _keyItemDatas;
        private readonly List<IClickable> _clickables = new List<IClickable>();
        private bool enable = true;

        private void OnEnable()
        {
            _characterController._inputService.InputSystemActions.Player.Interact.performed += HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Interact.canceled += HandleCanceled;

            _triggerDetector.onTriggerEntered += HandleTriggerEnter;
            _triggerDetector.onTriggerExited += HandleExit;
        }

        private void HandleExit(Collider other)
        {
            if (other.TryGetComponent<IHighlightable>(out var highlightable))
            {
                highlightable.RemoveHighlight();
                OnClickableExit?.Invoke();
            }

            if (other.TryGetComponent<IClickable>(out var clickable))
            {
                _clickables.Remove(clickable);
            }

            Debug.Log($"Exited: {other.name}");
        }

        private void HandleTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHighlightable>(out var highlightable))
            {
                highlightable.Highlight();
                OnClickableEnter?.Invoke();
            }

            if (other.TryGetComponent<IClickable>(out var clickable))
            {
                if (!_clickables.Contains(clickable))
                    _clickables.Add(clickable);
            }

            Debug.Log($"Entered: {other.name}");
        }

        private void OnDisable()
        {
            _characterController._inputService.InputSystemActions.Player.Interact.performed -= HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Interact.canceled -= HandleCanceled;
            
            _triggerDetector.onTriggerEntered -= HandleTriggerEnter;
            _triggerDetector.onTriggerExited -= HandleExit;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            if (!enable) return;

            // Чистим список от уничтоженных объектов
            _clickables.RemoveAll(c => c == null || ((MonoBehaviour)c) == null);

            if (_clickables.Count == 0) return;

            // Находим ближайший объект
            var clickable = GetClosestClickable();
            if (clickable == null) return;

            var info = clickable.Click();
            if(info.playPickUpSound)
                RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_PickUp");

            if (info.hideClickableView)
                OnClickableExit?.Invoke();
        }

        private IClickable GetClosestClickable()
        {
            if (_clickables.Count == 0) return null;

            IClickable closest = null;
            float minDistance = float.MaxValue;

            foreach (var clickable in _clickables)
            {
                if (clickable == null) continue;

                var clickableTransform = ((MonoBehaviour)clickable).transform;
                float distance = Vector3.Distance(transform.position, clickableTransform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = clickable;
                }
            }

            return closest;
        }

        public void AddKey(KeyItemData keyItemData)
        {
            _keyItemDatas.Add(keyItemData);
        }

        private void HandleCanceled(InputAction.CallbackContext obj) { }

        public void Enable() => enable = true;
        public void Disable() => enable = false;
    }

    public interface IClickable
    {
        public ClickResult Click();
    }

    public struct ClickResult
    {
        public bool hideClickableView;
        public bool playPickUpSound;
        public ClickResult(bool hideClickableView, bool playPickUpSound = true)
        {
            this.hideClickableView = hideClickableView;
            this.playPickUpSound = playPickUpSound;
        }
    }
}