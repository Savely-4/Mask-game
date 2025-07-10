using Cysharp.Threading.Tasks;
using System.Threading;

namespace Runtime.Services.StateMachine
{
    public abstract class State
    {
        protected readonly StateMachine _stateMachine;
        private CancellationTokenSource _updateCts;

        public State(StateMachine stateMachine) 
        {
            _stateMachine = stateMachine;
        }

        public async UniTask OnEnterAsync()
        {
            _updateCts = new CancellationTokenSource();

            await OnEnterAsyncInState();

            CancellationToken token = _updateCts.Token;
            _ = RunUpdateLoop(token);
        }

        protected virtual async UniTask OnEnterAsyncInState()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask OnExitAsync()
        {
            _updateCts?.Cancel();
            _updateCts?.Dispose();
            _updateCts = null;

            await OnExitAsyncInState();
        }

        protected virtual async UniTask OnExitAsyncInState()
        {
            await UniTask.CompletedTask;
        }

        private async UniTask RunUpdateLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                OnUpdate();
            }
        }

        protected abstract void OnUpdate();
    }
}
