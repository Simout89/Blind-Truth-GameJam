using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Interactable.Debug
{
    public class QTETrigger: MonoBehaviour
    {
        [Inject] private QuickTimeEvent _quickTimeEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                _quickTimeEvent.StartQTE();
            }
        }
    }
}