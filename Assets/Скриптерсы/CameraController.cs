using System;
using UnityEngine;

namespace Скриптерсы
{
    public class CameraController: MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}