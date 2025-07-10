using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Runtime.Services.StateMachine
{
    public class TestStateMachine : StateMachine
    {
        public TestStateMachine()
        {
            states = new Dictionary<Type, State>
            {
                { typeof(TestState1), new TestState1(this) },
                { typeof(TestState2), new TestState2(this) },
                { typeof(TestState3), new TestState3(this) }
            };

            SetState<TestState1>().Forget();
        }
    }
}