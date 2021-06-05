using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ModelFadeMotion : EnterExitModule,IFadeMotion
{

    [SerializeField] protected float inDuration = 1;
    [SerializeField] protected float outDuration = 1;
    [SerializeField] protected Ease inEase = Ease.InExpo;
    [SerializeField] protected Ease outEase = Ease.OutExpo;
    [SerializeField] protected float targetAlpha = 1;
    
    protected  MeshRenderer model;

    protected override void Initialize()
    {
        
        model = GetComponentInChildren<MeshRenderer>();
    }

    public override Sequence Enter(float timescale = 1, bool activate = true)
    {
        var ENTER = base.EnterInitiailzer(timescale, activate);

        ENTER.Append(model.material.DOFade(targetAlpha, inDuration).SetEase(inEase));

        if (activate)
        {
            ENTER.Play();
        }

        return ENTER;
    }


    public override Sequence Exit(float timescale = 1, bool activate = true)
    {
        var EXIT = base.ExitInitializer(timescale, activate);
        EXIT.Append(model.material.DOFade(0, outDuration).SetEase(outEase));

        if (activate)
        {
            EXIT.Play();
        }

        return EXIT;
    }
}
