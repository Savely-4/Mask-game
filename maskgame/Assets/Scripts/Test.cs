using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

public class Test : MonoBehaviour
{
    private UniTaskHandler countHandler;
    private int _count = 0;
    
    void Awake()
    {
        
        //Example 1
        // countHandler = new UniTaskHandler(new Func<CancellationToken, UniTask>[]
        // {
            // token => StartCount(token),
            // token => StartCount(token)
        // });
        
        //Example 2
        countHandler = new UniTaskHandler(StartCount);
        
        countHandler.Start();
    }

    async UniTask StartCount(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Debug.Log($"Thread: {_count}, Time {Time.time}");
            await UniTask.Delay(TimeSpan.FromSeconds(_count), cancellationToken: token);
        }
    }
}
