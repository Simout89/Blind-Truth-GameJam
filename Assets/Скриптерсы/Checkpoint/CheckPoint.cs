using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы
{
    public class CheckPoint: MonoBehaviour
    {
        [Inject] private CheckPointManager checkPointManager;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.GetComponent<CharacterController>())
                return;
            
            checkPointManager.Save();
            
            gameObject.SetActive(false);
        }
    }
}