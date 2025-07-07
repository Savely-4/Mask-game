using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Runtime.Services.StateMachine
{
    public class TestState1 : State
    {
        public TestState1(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override async UniTask OnEnterAsync()
        {
            Debug.Log("TestState1.OnEnter(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("TestState1.OnEnter(): Готово");
        }

        public override async UniTask OnExitAsync()
        {
            Debug.Log("TestState1.OnExit(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("TestState1.OnExit(): Готово");
        }

        public override void OnUpdate()
        {
            Debug.Log("TestState1.OnUpdate().Begin");

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _stateMachine.SetState<TestState2>();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _stateMachine.SetState<TestState3>();
            }

            Debug.Log("TestState1.OnUpdate().End");
        }
    }

    public class TestState2 : State
    {
        public TestState2(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override async UniTask OnEnterAsync()
        {
            Debug.Log("TestState2.OnEnter(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            Debug.Log("TestState2.OnEnter(): Готово");
        }

        public override async UniTask OnExitAsync()
        {
            Debug.Log("TestState2.OnExit(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            Debug.Log("TestState2.OnExit(): Готово");
        }

        public override void OnUpdate()
        {
            Debug.Log("TestState2.OnUpdate().Begin");

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _stateMachine.SetState<TestState1>();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _stateMachine.SetState<TestState3>();
            }

            Debug.Log("TestState2.OnUpdate().End");
        }
    }

    public class TestState3 : State
    {
        public TestState3(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override async UniTask OnEnterAsync()
        {
            Debug.Log("TestState3.OnEnter(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            Debug.Log("TestState3.OnEnter(): Готово");
        }

        public override async UniTask OnExitAsync()
        {
            Debug.Log("TestState3.OnExit(): Входим в состояние...");
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            Debug.Log("TestState3.OnExit(): Готово");
        }

        public override void OnUpdate()
        {
            Debug.Log("TestState3.OnUpdate().Begin");

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _stateMachine.SetState<TestState1>();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _stateMachine.SetState<TestState2>();
            }

            Debug.Log("TestState3.OnUpdate().End");
        }
    }
}
