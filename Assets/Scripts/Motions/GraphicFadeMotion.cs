using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicFadeMotion : EnterExitModule,IFadeMotion
{

    [SerializeField] protected float inDuration = 1;
    [SerializeField] protected float outDuration = 1;
    [SerializeField] protected Ease inEase = Ease.InExpo;
    [SerializeField] protected Ease outEase = Ease.OutExpo;
    [SerializeField] protected float targetAlpha = 1;
    
    protected  Graphic panel;

    protected override void Initialize()
    {
        panel = GetComponentInChildren<Graphic>();
    }

    public override Sequence Enter(float timescale = 1, bool activate = true)
    {
        var ENTER = base.EnterInitiailzer(timescale, activate);

        ENTER.Append(panel.DOFade(targetAlpha, inDuration).SetEase(inEase));

        if (activate)
        {
            ENTER.Play();
        }

        return ENTER;
    }


    public override Sequence Exit(float timescale = 1, bool activate = true)
    {
        var EXIT = base.ExitInitializer(timescale, activate);
        EXIT.Append(panel.DOFade(0, outDuration).SetEase(outEase));

        if (activate)
        {
            EXIT.Play();
        }

        return EXIT;
    }
}
