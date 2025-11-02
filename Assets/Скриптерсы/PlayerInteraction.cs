using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Скриптерсы
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        public event Action OnClickableEnter;
        public event Action OnClickableExit;

        private readonly List<IClickable> _clickables = new List<IClickable>();
        private bool enable = true;

        private void OnEnable()
        {
            _characterController._inputService.InputSystemActions.Player.Interact.performed += HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Interact.canceled += HandleCanceled;
        }

        private void OnDisable()
        {
            _characterController._inputService.InputSystemActions.Player.Interact.performed -= HandlePerformed;
            _characterController._inputService.InputSystemActions.Player.Interact.canceled -= HandleCanceled;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            if (!enable) return;

            // Чистим список от уничтоженных объектов
            _clickables.RemoveAll(c => c == null || ((MonoBehaviour)c) == null);

            if (_clickables.Count == 0) return;

            var clickable = _clickables[0];
            if (clickable == null) return; // ещё одна защита

            var info = clickable.Click();
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_PickUp");

            if (info.hideClickableView)
                OnClickableExit?.Invoke();
        }

        private void HandleCanceled(InputAction.CallbackContext obj) { }

        private void OnTriggerEnter(Collider other)
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

        private void OnTriggerExit(Collider other)
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
        public ClickResult(bool hideClickableView)
        {
            this.hideClickableView = hideClickableView;
        }
    }
}
