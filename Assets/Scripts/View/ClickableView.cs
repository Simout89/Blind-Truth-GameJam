using System;
using UnityEngine;

namespace Скриптерсы.View
{
    public class ClickableView : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private GameObject sign;

        private int _clickableCount; // счётчик активных кликабельных объектов

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            _playerInteraction.OnClickableEnter += HandleEnter;
            _playerInteraction.OnClickableExit += HandleExit;
        }

        private void OnDisable()
        {
            _playerInteraction.OnClickableEnter -= HandleEnter;
            _playerInteraction.OnClickableExit -= HandleExit;
        }

        private void HandleEnter()
        {
            _clickableCount++;
            Show();
        }

        private void HandleExit()
        {
            _clickableCount = Mathf.Max(0, _clickableCount - 1);
            if (_clickableCount == 0)
                Hide();
        }

        private void Show() => sign.SetActive(true);
        private void Hide() => sign.SetActive(false);
    }
}