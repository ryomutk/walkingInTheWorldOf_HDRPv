using DG.Tweening;
using UnityEngine;
using System.Collections;
using Utility;
using ModulePattern;

public class UFOMoveMotion : MonoBehaviour, IMoveMotion
{
    [SerializeField] float duration = 1;
    [SerializeField] Ease inEase = Ease.InSine;
    [SerializeField] Ease outEase = Ease.OutBounce;
    [SerializeField] Vector3 inRotate = new Vector3(10, 0, 0);
    [SerializeField] Vector3 defRotate = new Vector3(0, 0, 0);


    public float moveTime { get { return duration; } }

    /// <summary>
    /// 1進ンにかかる時間
    /// </summary>
    [SerializeField] float moveDuration;
    FloatMotion floatMotion;
    ModuleState state;
    bool stopping = false;
    System.Action<string> log;

    void Start()
    {
        log = (x) => LogWriter.Log("UFO MOTION: " + x, "TweenLog");
        floatMotion = GetComponent<FloatMotion>();
    }

    Tween moveTw;


    [Sirenix.OdinInspector.Button]
    public bool Move(Vector3 direction)
    {
        if (moveTw == null || !moveTw.active)
        {
            Enter();
            moveTw = transform.parent.DOMove(direction, moveDuration).SetEase(Ease.Linear).SetRelative();
            moveTw.Play();
        }
        else
        {
            StartCoroutine(ContinuousRoutine(direction));
        }

        return true;
    }

    /// <summary>
    /// 初動とかないやつ
    /// </summary>
    /// <param name="direction"></param>
    public bool Slide(Vector3 direction)
    {
        var tw = transform.parent.DOMove(direction, moveDuration).SetEase(Ease.Linear).SetRelative();


        tw.Play();


        return true;
    }


    [Sirenix.OdinInspector.Button]
    public bool Stop()
    {
        log("STOP COMMAND");
        if (moveTw != null && moveTw.active && !stopping)
        {
            StartCoroutine(StopRoutine());
        }

        Exit();


        return false;
    }

    IEnumerator StopRoutine()
    {
        log("STOPPING");
        stopping = true;
        var nowLoops = moveTw.CompletedLoops();

        yield return moveTw.WaitForElapsedLoops(nowLoops + 1);

        moveTw.Kill();
        stopping = false;
        log("STOP COMPLEATE");
    }

    IEnumerator ContinuousRoutine(Vector3 direction)
    {
        log("CONTINUE");
        if (moveTw != null && !stopping)
        {
            yield return StartCoroutine(StopRoutine());
        }
        else
        {
            yield return new WaitWhile(() => stopping);
        }

        Move(direction);

    }


    Sequence Enter(float timescale = 1, bool activate = true)
    {
        var ENTER = DOTween.Sequence();
        ENTER.Append(transform.DOLocalRotate(inRotate, duration)).SetEase(inEase)
        .onPlay += () => floatMotion.Stop();

        if (activate)
        {
            ENTER.Play();
        }

        return ENTER;

    }

    Sequence Exit(float timescale = 1, bool activate = true)
    {
        var EXIT = DOTween.Sequence();
        EXIT.Append(transform.DOLocalRotate(defRotate, duration)).SetEase(outEase)
        .onPlay += () => floatMotion.Begin();

        if (activate)
        {
            EXIT.Play();
        }

        return EXIT;
    }
}