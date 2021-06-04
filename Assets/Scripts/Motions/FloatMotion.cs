using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatMotion : MonoBehaviour
{
    [SerializeField] float top = 0.2f;
    [SerializeField] float bottom = -0.2f;
    [SerializeField] float duration = 1;


    Sequence idleMotion;


    [Sirenix.OdinInspector.Button]
    public Sequence Begin(bool active = true)
    {

        var UP = transform.DOLocalMoveY(top, duration / 2).SetEase(Ease.OutSine);
        var DOWN = transform.DOLocalMoveY(bottom, duration).SetEase(Ease.InOutSine);
        var MIDDLE = transform.DOLocalMoveY(0, duration / 2).SetEase(Ease.InSine);


        idleMotion = DOTween.Sequence();

        idleMotion.Append(UP)
        .Append(DOWN)
        .Append(MIDDLE);
        //seq.Append(down);

        idleMotion.SetLoops(-1);

        if (active)
        {
            idleMotion.Play();
        }

        return idleMotion;
    }

    [Sirenix.OdinInspector.Button]
    public void Stop()
    {
        if (idleMotion != null && idleMotion.active)
        {
            StartCoroutine(StopCoroutine());
        }
    }

    IEnumerator StopCoroutine()
    {
        var nowLoops = idleMotion.CompletedLoops();
        yield return idleMotion.WaitForElapsedLoops(nowLoops + 1);
        idleMotion.Kill();
    }
}
