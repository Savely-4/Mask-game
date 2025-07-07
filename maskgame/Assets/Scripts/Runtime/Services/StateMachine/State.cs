using Cysharp.Threading.Tasks;

namespace Runtime.Services.StateMachine
{
    public abstract class State
    {
        protected readonly StateMachine _stateMachine;

        public State(StateMachine stateMachine) 
        {
            _stateMachine = stateMachine;
        }

        public virtual UniTask OnEnterAsync() => UniTask.CompletedTask;

        public virtual UniTask OnExitAsync() => UniTask.CompletedTask;

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }
    }
}
