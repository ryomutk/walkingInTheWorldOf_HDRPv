using UnityEngine;
using System.Collections.Generic;

//自分を投げるSubjectを特定のObserver<M>に投げる
public abstract class MediatorBase<T> : IMediator<T>
{
    protected List<IObserverBase> observerList = new List<IObserverBase>();
    protected List<IObserverBase> simpleNoticedList = new List<IObserverBase>();
    protected Logger logger;

    public MediatorBase()
    {
        logger = new Logger("SystemLog", this.ToString());
    }

    public void SimpleNotice<M>(M data)
    {
        for (int i = 0; i < observerList.Count; i++)
        {
            if (observerList[i] is IObserver<M> observer)
            {
                observer.OnNotice(data);

                //SimpleNoticeは特殊なので、これは別のNoticeで使われない必要もある。その判定用にサンプルをとる。
                if (!simpleNoticedList.Contains(observer))
                {
                    simpleNoticedList.Add(observer);
                }
            }
        }
    }

    /// <summary>
    /// TからIObasrverBaseが求めるものを抽出してNotice。具体的な変換を行う。
    /// NoticeAllで使われることを前提としている
    /// 本体を通知するタイプ、および何も値を渡さないタイプは基底クラスで実装済み
    /// </summary>
    /// <param name="source"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    protected virtual bool ConvertNotice(T source, IObserverBase observer)
    {
        //引数がいらないタイプなら
        if (observer is IObserver observer1)
        {
            observer1.OnNotice();
            return true;
        }
        else if (observer is IObserver<T> observer2)
        {
            observer2.OnNotice(source);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 合致する型(無型)のObserverに通知を送る
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="M"></typeparam>
    public void OnNotice(T source)
    {
        foreach (var obs in observerList)
        {
            if (obs is IObserver obs1)
            {
                ConvertNotice(source, obs1);
            }
        }
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
                ConvertNotice(source, obs1);
            }
        }
    }

    /// <summary>
    /// 登録されているすべてに正しい通知を送る
    /// </summary>
    /// <param name="source"></param>
    public void NoticeAll(T source)
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


    //以下二つを公開してしまうとなんでも追加できてしまうので
    //あんまり使ってほしくないが
    //正直新しく作るのは面倒なので
    //やっぱ公開しちゃう

    public bool AddObserver(IObserverBase observerBase)
    {
        if (!observerList.Contains(observerBase))
        {
            observerList.Add(observerBase);
            return true;
        }


        return false;
    }

    public bool RemoveObserver(IObserverBase observerBase)
    {
        if (observerList.Contains(observerBase))
        {
            observerList.Remove(observerBase);
            return true;
        }

        return false;
    }
}