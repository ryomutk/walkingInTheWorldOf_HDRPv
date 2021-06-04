using UnityEngine;
using System.Collections.Generic;

//自分を投げるSubjectを特定のObserver<M>に投げる
public abstract class MediatorBase<T>
{
    protected List<IObserverBase> observerList = new List<IObserverBase>();
    protected Logger logger;

    public MediatorBase()
    {
        logger = new Logger("SystemLog", this.ToString());
    }

    //TからIObasrverBaseが求めるものを抽出してNotice
    //継承クラスではこれをoverrideして値を抽出
    protected virtual bool ConvertNotice(T source, IObserverBase observer)
    {
        //引数がいらないタイプなら
        if (observer is IObserver observer1)
        {
            observer1.OnNotice();
            return true;
        }

        return false;
    }

    
    /// <summary>
    /// 合致する型のObserverに通知を送る
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="M"></typeparam>
    public void OnNotice<M>(T source)
    {
        foreach (var obs in observerList)
        {
            if (obs is IObserver<M> obs1)
            {
                OnNotice<M>(source, obs1);
            }
        }
    }

    /// <summary>
    /// 登録されているすべてに正しい通知を送る
    /// </summary>
    /// <param name="source"></param>
    public void OnNotice(T source)
    {
        foreach (var obs in observerList)
        {
            //変換に失敗したら
            if (!ConvertNotice(source, obs))
            {
                logger.LogError("CONVERTION ERROR!");
            }
        }
    }

    /// <summary>
    /// 特定のobserverを指定して通知を送信。
    /// 具体的な型変換はここで行う。
    /// </summary>
    /// <param name="source"></param>
    /// <param name="observer"></param>
    /// <typeparam name="M"></typeparam>
    protected abstract bool OnNotice<M>(T source, IObserver<M> observer);

    //以下二つを公開してしまうとなんでも追加できてしまうので
    //継承先は必要なだけISubjectを追加してこれらへのインターフェイスを設ける

    protected bool AddObserver(IObserverBase observer)
    {
        if (!observerList.Contains(observer))
        {
            observerList.Add(observer);
            return true;
        }


        return false;
    }

    protected bool RemoveObserver(IObserverBase observer)
    {
        if (observerList.Contains(observer))
        {
            observerList.Remove(observer);
            return true;
        }

        return false;
    }
}