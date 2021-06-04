using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 全ての移動するオブジェクトの基底となるクラス。
/// 決まった移動をするオブジェクトに使いやすいようにMotionBaseを拡張したモノ
/// 
/// MotionStateは動いている最中はIdleになります
/// 
/// 色々汎用性の高そうな動きも用意しようかと思ったが
/// いびつになるだけなのでForward以外作らない
/// よく使う動きとかはこれを継承したModuleかなんか作って対応してください
/// 
/// equenceの配列を保持し、
/// Registerで登録、
/// Triggerで順番に実行してく方式
/// </summary>
public class MovementModule : EnterExitModule
{
    List<Tween> SequenceQueue = new List<Tween>();

    [SerializeField] bool StartAfterEnter = true;

    //Movementが終わったら基本的にはお役御免になると考えて、
    //取り敢えずこれは基本的にTrueで
    //変更用のインターフェイスも設けない
    bool ResetQueueAfterExit = true;

    /// <summary>
    /// これ自体がdisableされちゃうと、おかしなことになりがち(弾そのものではなくmoduleだけがdisableされちゃったり)
    /// なので今はfalse
    /// </summary>
    bool DisableAfterExit = false;

    ModuleState _moduleState;

    /// <summary>
    /// Enter終わるまで:Disabled
    /// 動いている時:working
    /// 動き後:compleated(enter後も同様)
    /// Queue終了後:sleep
    /// になる
    /// 最初以外自動的にreadyにはならない
    /// </summary>
    [ShowInInspector]
    public ModuleState moduleState { get { return _moduleState; } }


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {

    }

    /// <summary>
    /// queueにSequenceを登録。
    /// </summary>
    /// <param name="tween"></param>
    /// <returns>現在登録されてるSequence数</returns>
    public int RegisterMotion(Tween tween)
    {
        tween.onPlay += () => _moduleState = ModuleState.working;
        tween.onComplete += () => MotionCompleate();
        SequenceQueue.Add(tween);

        return SequenceQueue.Count;
    }

    /// <summary>
    /// 一つ前のsequenceにjoin
    /// </summary>
    /// <param name="tween"></param>
    /// <returns></returns>
    public int RegisterJoin(Tween tween)
    {
        if (SequenceQueue.Count == 0)
        {
            Debug.LogWarning("no sequence to join!");
            return 0;
        }

        //新しいsequenceを作り
        var seq = DOTween.Sequence();

        //最後に登録されたtweenを取得し
        var lastTw = SequenceQueue[SequenceQueue.Count - 1];

        //sequenceとしてjoinし、
        seq.Append(lastTw).Join(tween);

        //置き換える
        SequenceQueue[SequenceQueue.Count - 1] = seq;

        return SequenceQueue.Count;
    }

    void MotionCompleate()
    {
        if (SequenceQueue.Count != 0)
        {
            _moduleState = ModuleState.compleate;
        }
        else
        {
            //もう後がない時はcompleateではなく、気絶。
            _moduleState = ModuleState.sleeping;
        }
    }


    /// <summary>
    /// 次のSequenceを始めさせる
    /// </summary>
    /// <returns></returns>
    public bool TriggerSequence()
    {
        if (CheckTrigger())
        {
            SequenceQueue[0].Play();
            SequenceQueue.RemoveAt(0);

            return true;
        }

        return false;
    }


    bool CheckTrigger()
    {

        if (SequenceQueue.Count == 0)
        {

            return false;
        }
        if (moduleState == ModuleState.compleate)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timescale"></param>
    /// <param name="activate"></param>
    /// <param name="thenIdle">効かないので注意</param>
    /// <returns></returns>
    public override Sequence Enter(float timescale = 1, bool activate = true)
    {
        var ENTER = EnterInitiailzer(timescale, activate);

        if (activate)
        {
            ENTER.Play();
        }

        return ENTER;
    }

    protected override Sequence EnterInitiailzer(float timescale = 1, bool activate = true)
    {
        //ここでidleが勝手に始まらないようにしている。
        var ENTER = base.EnterInitiailzer(timescale, activate);

        ENTER.onComplete += () => _moduleState = ModuleState.compleate;

        if (StartAfterEnter)
        {
            ENTER.onComplete += () => TriggerSequence();
        }

        return ENTER;
    }

    public override Sequence Exit(float timescale = 1, bool activate = true)
    {
        var EXIT = ExitInitializer(timescale, activate);

        if (activate)
        {
            EXIT.Play();
        }

        return EXIT;
    }


    protected override Sequence ExitInitializer(float timescale = 1, bool activate = true)
    {
        var EXIT = base.ExitInitializer(timescale, activate);

        if (DisableAfterExit)
        {
            EXIT.onComplete += () =>
            {
                gameObject.SetActive(false);
            };
        }

        if (ResetQueueAfterExit)
        {
            EXIT.onComplete += () => SequenceQueue.Clear();
        }

        EXIT.onComplete += () => _moduleState = ModuleState.disabled;

        return EXIT;
    }

    public void DeleteSequenceQueue()
    {
        SequenceQueue.Clear();
    }

}