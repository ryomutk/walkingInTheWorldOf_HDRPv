using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public abstract class MotionBase : EnterExitModule
{
    public Sequence IDLESequence = null;


    public virtual Sequence Enter(float timescale = 1, bool activate = true, bool thenIdle = true)
    {
        Debug.LogWarning("Enter is not implemented!");

        return null;
    }

    protected Sequence EnterInitializer(float timescale = 1, bool activate = true, bool thenIdle = true)
    {
        var ENTER = base.EnterInitiailzer(timescale, activate);

        if (thenIdle)
        {
            ENTER.onComplete += () => Idle();
        }

        return ENTER;
    }

    protected override Sequence ExitInitializer(float timescale = 1, bool activate = true)
    {
        var EXIT = base.ExitInitializer(timescale, activate);

        EXIT.onComplete += () =>
        {
            PauseIdle(); //これだけで大丈夫だろうか？
            _state = 0;
        };

        return EXIT;
    }

    ///<summary>
    ///UIを待機させるためのメソッド
    ///Idleだけはpauseとrestartで動かすので
    ///挙動はSetIdleで設定してください。
    ///activateの時のplay実装忘れず
    ///Idleのみ実装されてなくてもnullを返さない。今のとこ</summary>
    [Button]
    public Sequence Idle(float timescale = 1, bool activate = true)
    {

        if (IDLESequence == null)
        {
            SetIdle();
            SetStateChanger(IDLESequence, MotionState.Idling);
        }

        IDLESequence.timeScale = timescale;
        IDLESequence.Play();


        return IDLESequence;
    }

    protected virtual Sequence SetIdle(LoopType loopType = default)
    {
        IDLESequence = DOTween.Sequence();
        IDLESequence.SetLoops(-1, loopType);

        return IDLESequence;
    }

    public bool PauseIdle()
    {
        if (IDLESequence != null && IDLESequence.IsPlaying())
        {
            IDLESequence.Pause();
            return true;
        }

        return false;
    }

    [Button]
    ///<summary>
    ///画面から消したくないけど退避させたいときのメソッド</summary>
    public virtual Sequence Hide(float timescale = 1, bool activate = true, bool pauseIdle = true)
    {
        Debug.LogWarning("Hide is not implemented!");

        return null;
    }

    protected virtual Sequence HideInitializer(float timescale = 1, bool activate = true, bool pauseIdle = true)
    {
        var HIDE = DOTween.Sequence();

        HIDE.timeScale = timescale;

        if (pauseIdle)
        {
            HIDE.onComplete += () =>
            {
                PauseIdle();
                FlagManager<MotionState>.RemoveFlag(ref _state, MotionState.Hiding);
                FlagManager<MotionState>.AppendFlag(ref _state, MotionState.Hidden);
            };
        }

        SetStateChanger(HIDE, MotionState.Hiding);

        return HIDE;
    }

    [Button]
    public virtual Sequence Resume(float timescale = 1, bool activate = true, bool thenIdle = true)
    {
        return null;
    }

    protected virtual Sequence ResumeInitializer(float timescale = 1, bool activate = true, bool thenIdle = true)
    {
        if (_state.HasFlag(MotionState.Hidden))
        {
            var RESUME = DOTween.Sequence();
            RESUME.timeScale = timescale;

            if (thenIdle)
            {
                RESUME.onComplete += () => Idle();
            }

            RESUME.onComplete += () => FlagManager<MotionState>.RemoveFlag(ref _state, MotionState.Hidden);

            SetStateChanger(RESUME, MotionState.Resuming);

            return RESUME;
        }
        return null;
    }
}
