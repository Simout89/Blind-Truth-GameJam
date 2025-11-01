using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Services
{
    public class InputService: IInputService, IDisposable, IInitializable
    {
        public InputSystem_Actions InputSystemActions { get; private set; }
        
        public void Initialize()
        {
            InputSystemActions = new InputSystem_Actions();
            InputSystemActions.Enable();
            
            Debug.Log("1234");
            
        }
        
        public void Dispose()
        {
            InputSystemActions.Disable();
        }

    }

    public interface IInputService
    {
        public InputSystem_Actions InputSystemActions { get;}
    }
}