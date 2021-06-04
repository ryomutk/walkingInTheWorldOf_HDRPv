using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// スプライトを動かすためのTweenを生成してくれるクラス。
/// </summary>
public class SpriteSwapper
{
    SpriteRenderer renderer;

    /// <summary>
    /// swap中のスプライトのインデックスを保持しておくリスト。
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <returns></returns>
    List<int> indexList = new List<int>();

    public SpriteSwapper(SpriteRenderer SR)
    {
        renderer = SR;
    }

    public Tween DoSwap(Sprite[] sprites, float duration, AnimationCurve ease)
    {
        int idx = 0;
        int count = indexList.Count;
        indexList.Add(idx);
        var tw = DOTween.To(() => indexList[count], (x) =>
         {
             Debug.Log(x);
             indexList[count]++;
             SwapTo(sprites, x);
         }, sprites.Length, duration).SetEase(ease);



        return tw;

    }

    public Tween DoSwap(Sprite[] sprites, float duration, Ease ease)
    {
        int idx = 0;
        int count = indexList.Count;
        indexList.Add(idx);
        var tw = DOTween.To(() => indexList[count], (x) =>
         {
             Debug.Log(x);
             indexList[count]++;
             SwapTo(sprites, x);
         }, sprites.Length - 1, duration).SetEase(ease);



        return tw;
    }

    public bool SwapTo(Sprite[] sprites, int index)
    {
        try
        {
            renderer.sprite = sprites[index];
            return true;
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
    }
}
