using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class FlashMotion : GraphicFadeMotion
{

    [Button]
    public Sequence Flash(bool activate)
    {
        var SEQ = DOTween.Sequence();
        SEQ.Append(Enter(activate:false))
        .Append(Exit(activate:false));

        if(activate){
            SEQ.Play();
        } 

        return SEQ;
    }
}