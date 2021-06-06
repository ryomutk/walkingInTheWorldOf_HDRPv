using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using DG.Tweening;

namespace Motion
{
    public class Easer
    {
        float overshootOrAnph =1.70158f;
        float period = 0;
        Dictionary<Ease,EaseFunction> easeFuncList;
        
        public float Ease(float time,float duration,Ease ease)
        {
            EaseFunction func;
            if(!easeFuncList.TryGetValue(ease,out func))
            {
                easeFuncList[ease] = EaseManager.ToEaseFunction(ease);
            }

            return func(time,duration,overshootOrAnph,period);
        }
    }
}