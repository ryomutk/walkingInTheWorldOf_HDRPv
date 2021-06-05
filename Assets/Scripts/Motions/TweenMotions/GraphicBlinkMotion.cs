using UnityEngine;
using DG.Tweening;

public class GraphicBlinkMotion:GraphicFadeMotion
{
    Sequence blinkSeq = null;

    [Sirenix.OdinInspector.Button]
    public void StopBlink(){
        Debug.Log("kil");
        blinkSeq.Kill();
        blinkSeq = null;
        Exit();
    }

    [Sirenix.OdinInspector.Button]
    public Sequence Blink(bool activate){
        
        if(blinkSeq != null)
        return null;

        blinkSeq = DOTween.Sequence();
        blinkSeq.Append(Enter(activate:false))
        .Append(Exit(activate:false));

        blinkSeq.SetLoops(-1);

        blinkSeq.Play();

        return blinkSeq;
    }
}