using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WorldFadeMotion : GraphicFadeMotion,IFadeMotion
{

    public static WorldFadeMotion instance = null;

    protected override void Awake()
    {
        base.Awake();
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this as WorldFadeMotion;
        }
        else
        {
            Debug.LogWarning(this.GetType() + "is singleton! now been killed");
            Destroy(this.gameObject);
        }
    }


    void OnDestroy()
    {
        instance = null;
    }
}
