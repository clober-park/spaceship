using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

public class Test : MonoBehaviour
{
    bool isactive = false;
    CancellationTokenSource cts = new CancellationTokenSource();
    // Start is called before the first frame update
    async void Start()
    {
        var result = testCall();
        if (result)
        {
                await testCall3();
        }   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            cts.Cancel();
        }
    }


   bool testCall ()
    {
        Debug.Log("test call Forget");
        try {
            testCall2().Forget();
            Debug.Log("test call Forget end");
            return true;
        }
        catch(Exception e) {
            Debug.LogException(e);
        }
        return false;
    }

    async UniTaskVoid testCall2 ()
    {
        Debug.Log("test call Delay");
        await UniTask.Delay(10000, cancellationToken: this.GetCancellationTokenOnDestroy());
        isactive = true;
        Debug.Log("waited test call Delay");
    }
    async UniTask testCall3 ()
    {
        Debug.Log(string.Format("test call waituntil isactive is {0}", isactive));
        await Loading();
        await UniTask.WaitUntil(() => isactive == true);
        Debug.Log(string.Format("waited test call waituntil isactive is {0}", isactive));

        var ex = await testCall4(cts.Token);
        if (ex is OperationCanceledException)
        {
            // errr
        }
        else if (ex is not null)
        {
            //
        }
        Debug.Log("Waited test caneledexception unitask input skip end");
    }
    
    async UniTask<Exception> testCall4 (CancellationToken token = default)
    {
        Debug.Log("Call4 Start");
        if (token.IsCancellationRequested)
        {
            throw new OperationCanceledException(token);
        }
        Debug.Log("Call4 Pass Delay");
        await UniTask.Delay(10000, cancellationToken: token);
        Debug.Log("Call4 Pass Delay Pass");
        await DelayIgnore().WithCancellation(token);
        Debug.Log("waited test input skip canceledexception");
        return null;
    }

    public IEnumerator DelayIgnore() => UniTask.ToCoroutine(async () =>
    {
        var time = Time.realtimeSinceStartup;

        Time.timeScale = 0.5f;
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), ignoreTimeScale: true);

            var elapsed = Time.realtimeSinceStartup - time;
            Assert.AreEqual(3, (int)Math.Round(TimeSpan.FromSeconds(elapsed).TotalSeconds, MidpointRounding.ToEven));
        }
        finally
        {
            Time.timeScale = 1.0f;
        }
    });

    public IEnumerator Loading() => UniTask.ToCoroutine(async () =>
    {
        while (true)
        {
            Debug.Log("Loading...");
            await UniTask.Delay(200, ignoreTimeScale: false);

            if (isactive)
                return;
        }
    });

    public class Sample
    {
        public int health;
        public Sample sample;
        public Sample (int defaultHealth, Sample sample)
        {
            //Assert ¿¹½Ã
            Assert.IsNotNull(sample);
            Assert.AreNotEqual(0, defaultHealth);
            health = defaultHealth;
            this.sample = sample;
        }
    }

    public class Client
    {

        void Call()
        {
            Sample s = new Sample(0, null);
        }        
    }
}
