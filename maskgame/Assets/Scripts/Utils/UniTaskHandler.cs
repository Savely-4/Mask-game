using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class UniTaskHandler : IDisposable
    {
        private readonly IEnumerable<Func<CancellationToken, UniTask>> _tasks;
        private CancellationTokenSource _cts;
        private List<UniTask> _runningTasks = new();

        public UniTaskHandler(IEnumerable<Func<CancellationToken, UniTask>> tasks)
        {
            this._tasks = tasks;
        }

        public UniTaskHandler(Func<CancellationToken, UniTask> task) : this(new[] { task }) {}

        public void Start()
        {
            Stop();
            
            _cts = new CancellationTokenSource();
            _runningTasks = new List<UniTask>();

            foreach (var task in _tasks)
            {
                _runningTasks.Add(task(_cts.Token));
            }
            
            foreach (var runningTask in _runningTasks)
            {
                runningTask.Forget();
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            Dispose();
            _cts = null;
            
            _runningTasks.Clear();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}
