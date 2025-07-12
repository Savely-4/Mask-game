using Cysharp.Threading.Tasks;
using Runtime.Services.StateMachine;

public class IdleState : State
{
    IdleState(StateMachine stateMachine) : base(stateMachine) { }
    protected override void OnUpdate()
    {
        
    }
}
