using DG.Tweening;
using UnityEngine;
using System.Collections;

public class UFOMoveMotion : MonoBehaviour
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
        log = (x) => LogWriter.Log("UFO MOTION: " + x,"TweenLog");
        floatMotion = GetComponent<FloatMotion>();
    }

    Tween moveTw;


    [Sirenix.OdinInspector.Button]
    public void Move(Vector3 direction)
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
    }

    /// <summary>
    /// 初動とかないやつ
    /// </summary>
    /// <param name="direction"></param>
    public Tween Slide(Vector3 direction, bool activate = true)
    {
        var tw = transform.parent.DOMove(direction, moveDuration).SetEase(Ease.Linear).SetRelative();

        if (activate)
        {
            tw.Play();
        }

        return tw;
    }


    [Sirenix.OdinInspector.Button]
    public void Stop(bool exit = true)
    {
        log("STOP COMMAND");
        if (moveTw != null && moveTw.active && !stopping)
        {
            StartCoroutine(StopRoutine());
        }

        if (exit)
        {
            Exit();
        }
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