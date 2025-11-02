using System;
using UnityEngine;

namespace Скриптерсы.View
{
    public class ClickableView: MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private GameObject sign;

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
            Show();
        }

        private void HandleExit()
        {
            Hide();
        }

        private void Show()
        {
            sign.SetActive(true);
        }

        private void Hide()
        {
            sign.SetActive(false);
        }
    }
}