using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 配置可能なブロック。
/// Mediatorを継承したほうがいろいろと楽になるが
/// このクラスを使うMediatorを継承すべきか検討中
/// 設計を変えるときはブランチ切ってね
/// </summary>
public abstract class BuildBlock:MonoBehaviour,ISubject<ModuleState>
{
    public abstract BlockType type{get;}
    protected BuildBlockMediator _mediator;

    void Start()
    {
        _mediator = new BuildBlockMediator();
    }


    /// <summary>
    /// working:レーダーなどがすべて動いている状態
    /// ready:可視だがレーダーなどは動いていない
    /// sleeping:不可視
    /// </summary>
    /// <value></value>
    public ModuleState state{get;private set;}

    //レーダーなどを動かす
    public void Activate()
    {
        state = ModuleState.working;
        _mediator.OnNotice<ModuleState>(this);
    }

    public void Disactivate()
    {
        state = ModuleState.ready;
        _mediator.OnNotice<ModuleState>(this);
    }



    public void Show()
    {
        state = ModuleState.ready;
        gameObject.layer = LayerMask.NameToLayer("Block");
    }

    public void Hide()
    {
        state = ModuleState.sleeping;
        gameObject.layer = LayerMask.NameToLayer("Invisible");
    }


    public bool AddObserver(IObserver<ModuleState> obs)
    {
        return _mediator.AddObserver(obs);
    }

    public bool RemoveObserver(IObserver<ModuleState> obs)
    {
        return _mediator.AddObserver(obs);
    }
}

public enum BlockType
{
    def,
    standard,
    stairs
}