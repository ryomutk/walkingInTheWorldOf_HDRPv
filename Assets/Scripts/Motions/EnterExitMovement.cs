using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public abstract class EnterExitModule : MonoBehaviour
{
    [SerializeField] bool _InitializeOnAwake = true;

    /// <summary>
    /// 最初にInitializeするかどうか
    /// 必ずする/しないなどの場合、overrideして下さい
    /// </summary>
    protected virtual bool InitializeOnAwake{get{return _InitializeOnAwake;}}
    
    
    protected MotionState _state = default;

    [ShowInInspector]
    public virtual MotionState state
    {
        get { return _state; }
    }

    protected virtual void Awake()
    {
        DOTween.Init();
        //作ってから別の場所で使う事も考えて自動では始まらないようにする
        DOTween.defaultAutoPlay = AutoPlay.None;
        DOTween.SetTweensCapacity(1250,1250);

        if (InitializeOnAwake)
        {
            Initialize();
        }
    }

    ///<summary>
    ///UIを画面に表示するためのメソッド
    ///activateの時のplay実装忘れず
    //仕様変更:子で実装されていない場合はnullを返す。初期化はInitializerで。</summary>
    [Button]
    public virtual Sequence Enter(float timescale = 1, bool activate = true)
    {
        Debug.LogWarning("Enter is not implemented!");

        return null;
    }

    protected virtual Sequence EnterInitiailzer(float timescale = 1, bool activate = true)
    {
        var ENTER = DOTween.Sequence();

        ENTER.timeScale = timescale;

        ENTER.onComplete += () => FlagManager<MotionState>.AppendFlag(ref _state,MotionState.Active);

        SetStateChanger(ENTER, MotionState.Entering);

        return ENTER;
    }


    ///<summary>
    ///UIを画面から排除するためのメソッド
    ///activateの時のplay実装忘れず
    ///仕様変更:子で実装されていない場合はnullを返す。初期化はInitializerで。</summary>
    [Button]
    public virtual Sequence Exit(float timescale = 1, bool activate = true)
    {
        Debug.LogWarning("Exit is not implemented!");
        return null;
    }

    protected virtual Sequence ExitInitializer(float timescale = 1, bool activate = true)
    {
        var EXIT = DOTween.Sequence();
        EXIT.timeScale = timescale;

        EXIT.onComplete += () => FlagManager<MotionState>.RemoveFlag(ref _state,MotionState.Active);

        SetStateChanger(EXIT, MotionState.Exiting);

        return EXIT;
    }

    protected void SetStateChanger(Sequence sequence, MotionState myState, TweenCallback customEndPoint = null)
    {
        sequence.onPlay += () =>
        {
            FlagManager<MotionState>.AppendFlag(ref _state, myState);
        };

        if (customEndPoint != null)
        {
            customEndPoint += () =>
            {
                FlagManager<MotionState>.RemoveFlag(ref _state, myState);
            };
        }
        else
        {
            sequence.onPause += () =>
            {
                FlagManager<MotionState>.RemoveFlag(ref _state, myState);
            };
            sequence.onComplete += () =>
            {
                FlagManager<MotionState>.RemoveFlag(ref _state, myState);
            };
        }
    }

    ///<summary>
    ///画像の初期化用。UIMotion内でAwakeに呼ばれる
    ///</summary>
    protected abstract void Initialize();


}