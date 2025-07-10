using Runtime.Services.StateMachine;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    private TestStateMachine _testStateMachine;

    private void Awake()
    {
        _testStateMachine = new TestStateMachine();
    }
}
