using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PanelOpenMotion : EnterExitModule
{
    [SerializeField] float enterDuration = 1;
    [SerializeField] float exitDuration = 1;
    [SerializeField] Ease enterEase = Ease.OutExpo;
    [SerializeField] Ease exitEase = Ease.InExpo;
    [SerializeField] float targetScale = 1;
    [SerializeField] float flashDelay = 0.7f;
    Graphic panel;
    FlashMotion flashFadeMotion;

    protected override void Awake()
    {
        base.Awake();
        panel = GetComponent<Graphic>();
        flashFadeMotion = GetComponentInChildren<FlashMotion>();
    }

    protected override void Initialize()
    {
        transform.DOScaleY(0, 0).Play();
    }

    [Button]
    public override Sequence Enter(float timescale = 1, bool activate = true)
    {
        var ENTER = base.EnterInitiailzer(timescale, activate);
        ENTER.Append(transform.DOScaleY(targetScale,enterDuration).SetEase(enterEase))
        .Join(flashFadeMotion.Exit(activate:false).SetDelay(flashDelay));

        


        if (activate)
        {
            ENTER.Play();
        }

        return ENTER;
    }

    [Button]
    public override Sequence Exit(float timescale = 1, bool activate = true)
    {
        var EXIT = base.ExitInitializer(timescale, activate);

        EXIT.Append(flashFadeMotion.Enter(activate:false))
        .Append(transform.DOScaleY(0,exitDuration).SetEase(exitEase));

        EXIT.SetEase(exitEase);

        if(activate){
            EXIT.Play();
        }

        return EXIT;
    }

}