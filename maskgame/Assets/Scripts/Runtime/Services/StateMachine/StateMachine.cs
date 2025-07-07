using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Services.StateMachine
{
    public abstract class StateMachine
    {
        protected Dictionary<Type, State> states;

        private State _currentState;
        private bool _isInTransition = false;

        public void OnUpdate()
        {
            _currentState?.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentState?.OnFixedUpdate();
        }

        public async void SetState<TState>() where TState : State
        {
            //if (_isInTransition)
            //{
            //    Debug.Log("Transition is not possible. Already in progress.");
            //    return;
            //}

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

        private TState GetState<TState>() where TState : State
        {
            return states[typeof(TState)] as TState;
        }
    }

}