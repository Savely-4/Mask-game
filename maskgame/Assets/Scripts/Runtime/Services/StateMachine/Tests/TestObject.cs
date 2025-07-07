using Runtime.Services.StateMachine;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    private TestStateMachine _testStateMachine;

    private void Awake()
    {
        _testStateMachine = new TestStateMachine();
    }

    private void Update()
    {
        _testStateMachine?.OnUpdate();
    }

    private void FixedUpdate()
    {
        _testStateMachine?.OnFixedUpdate();
    }
}
