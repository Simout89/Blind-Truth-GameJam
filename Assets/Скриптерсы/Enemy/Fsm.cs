using System;
using System.Collections.Generic;

namespace Скриптерсы.Enemy
{
    public class Fsm
    {
        private FsmState currentState;
        public FsmState CurrentState => currentState;
        private Dictionary<Type, FsmState> states = new Dictionary<Type, FsmState>();

        public void AddState(FsmState state)
        {
            states.Add(state.GetType(), state);
        }

        public void ChangeState<T>() where T : FsmState
        {
            var type = typeof(T);

            if (currentState != null && currentState.GetType() == type)
            {
                return;
            }

            if (states.TryGetValue(type, out var newState))
            {
                currentState?.Exit();

                currentState = newState;
                
                currentState?.Enter();
            }
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}
