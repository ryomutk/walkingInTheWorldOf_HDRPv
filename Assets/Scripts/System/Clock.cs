using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : Singleton<Clock>
{
    [SerializeField] int targetFrame = 60;
    [SerializeField] int tickRate = 10;
    public bool Signal{get;private set;}
    public int tickCount{get;private set;}
    public bool active{get;private set;}

    void Start()
    {
        Application.targetFrameRate = targetFrame;
        StartCoroutine(TickRoutine());
    }

    IEnumerator TickRoutine()
    {
        active = true;
        yield return null;

        while(true)
        {
            yield return tickRate;
            yield return new WaitUntil(()=>active);
            Signal = true;
            tickCount++;
            yield return null;
            Signal = false;
        }
    }

    public void Pause()
    {
        active = false;
    }
}
