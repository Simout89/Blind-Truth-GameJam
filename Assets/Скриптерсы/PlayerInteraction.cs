using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Скриптерсы
{
    public class PlayerInteraction: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        public event Action OnClickableEnter;
        public event Action OnClickableExit;

        private IClickable _clickable;

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
            if(!enable) return;

            if (_clickable != null)
            {
                var info = _clickable.Click();

                if (info.hideClickableView) OnClickableExit?.Invoke();
            }
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IHighlightable>(out var h))
            {
                h.Highlight();
                OnClickableEnter?.Invoke();
            }

            if (other.gameObject.TryGetComponent<IClickable>(out var c)) _clickable = c;
            
            
            Debug.Log(other.name);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IHighlightable>(out var h))
            {
                h.RemoveHighlight(); 
                OnClickableExit?.Invoke();
            }
            
            if (other.gameObject.TryGetComponent<IClickable>(out var c)) _clickable = null;
        }

        public void Enable()
        {
            enable = true;
        }

        public void Disable()
        {
            enable = false;
        }
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