using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Скриптерсы
{
    public class PlayerInteraction: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        public event Action OnClickableEnter;
        public event Action OnClickableExit;

        private List<IClickable> _clickables = new List<IClickable>();
        private List<IHighlightable> _highlightables = new List<IHighlightable>();

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

            CleanUpDestroyedObjects();

            if (_clickables.Count > 0)
            {
                var closestClickable = GetClosestClickable();
                if (closestClickable != null)
                {
                    var info = closestClickable.Click();

                    if (info.hideClickableView)
                    {
                        OnClickableExit?.Invoke();
                        
                        // Проверяем, остались ли еще объекты после взаимодействия
                        CleanUpDestroyedObjects();
                        if (_clickables.Count > 0 || _highlightables.Count > 0)
                        {
                            OnClickableEnter?.Invoke();
                        }
                    }
                }
            }
        }

        private void CleanUpDestroyedObjects()
        {
            _clickables.RemoveAll(c => c == null || (c is MonoBehaviour mb && mb == null));
            _highlightables.RemoveAll(h => h == null || (h is MonoBehaviour mb && mb == null));
        }

        private IClickable GetClosestClickable()
        {
            if (_clickables.Count == 0) return null;
            
            CleanUpDestroyedObjects();
            
            if (_clickables.Count == 0) return null;
            if (_clickables.Count == 1) return _clickables[0];

            IClickable closest = null;
            float minDistance = float.MaxValue;

            foreach (var clickable in _clickables)
            {
                if (clickable is MonoBehaviour mb && mb != null)
                {
                    float distance = Vector3.Distance(transform.position, mb.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = clickable;
                    }
                }
            }

            return closest;
        }

        private void HandleCanceled(InputAction.CallbackContext obj)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IHighlightable>(out var h))
            {
                if (!_highlightables.Contains(h))
                {
                    _highlightables.Add(h);
                    h.Highlight();
                    
                    if (_highlightables.Count == 1)
                    {
                        OnClickableEnter?.Invoke();
                    }
                }
            }

            if (other.gameObject.TryGetComponent<IClickable>(out var c))
            {
                if (!_clickables.Contains(c))
                {
                    _clickables.Add(c);
                }
            }
            
            Debug.Log(other.name);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IHighlightable>(out var h))
            {
                if (_highlightables.Contains(h))
                {
                    _highlightables.Remove(h);
                    h.RemoveHighlight();
                    
                    if (_highlightables.Count == 0)
                    {
                        OnClickableExit?.Invoke();
                    }
                }
            }
            
            if (other.gameObject.TryGetComponent<IClickable>(out var c))
            {
                _clickables.Remove(c);
            }
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