using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

//よく使うコルーチンまとめ
public class Corutines:Singleton<Corutines>
{
    //指定された状態になるまで待った後、指定された状態が終わるまで待つ。
    public delegate Coroutine CorutineHolder(IEnumerator enumerator);

    public static CorutineHolder staticStarter;
    //public static Action<Coroutine> staticStopper;

    protected override void Awake()
    {
        staticStarter = (x) => StartCoroutine(x);
        //staticStopper = (x) => StopCoroutine(x);
    }

    static public IEnumerator WaitAndRetry(Action action,int delay = 5){
        yield return delay;
        action();
    }

    static public IEnumerator SafeWaitWhile(Func<bool> wait)
    {
        yield return new WaitUntil(wait);
        yield return new WaitWhile(wait);
    }

    static public IEnumerator SafeWaitUntil(Func<bool> wait)
    {
        yield return new WaitUntil(wait);
        yield return new WaitWhile(wait);
    }
}
