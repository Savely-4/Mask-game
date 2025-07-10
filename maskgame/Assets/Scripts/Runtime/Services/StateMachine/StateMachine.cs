using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Runtime.Services.StateMachine
{
    public abstract class StateMachine
    {
        protected Dictionary<Type, State> states;

        private State _currentState;
        private bool _isInTransition = false;
        private CancellationTokenSource _cts;

        public async UniTask SetState<TState>() where TState : State
        {
            if (_isInTransition)
            {
                _cts?.Cancel();
                Debug.Log("Transition is not possible. Already in progress. Cancel.");
                return;
            }

            _isInTransition = true;
            _cts = new CancellationTokenSource();

            try
            {
                if (_currentState != null)
                {
                    await _currentState.OnExitAsync();
                }

                _currentState = GetState<TState>();

                if (_currentState != null)
                {
                    await _currentState.OnEnterAsync();
                }
            }
            finally
            {
                _isInTransition = false;
                _cts.Dispose();
                _cts = null;
            }

        }

        private TState GetState<TState>() where TState : State
        {
            return states[typeof(TState)] as TState;
        }
    }

}