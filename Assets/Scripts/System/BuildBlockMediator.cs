using UnityEngine;

public class BuildBlockMediator : MediatorBase<BuildBlock>,ISubject<ModuleState>
{


    protected override bool OnNotice<M>(BuildBlock source, IObserver<M> observer)
    {

        switch (observer)
        {
            case IObserver<ModuleState> obs:
                obs.OnNotice(source.state);
                return true;
        }

        logger.LogError("Notice method for TYPE:"+typeof(M) + "is not Implemented!");
        return false;
    }

    protected override bool ConvertNotice(BuildBlock source, IObserverBase observer)
    {
        if (!base.ConvertNotice(source, observer))
        {
            if (observer is IObserver<ModuleState> obs)
            {
                OnNotice<ModuleState>(source, obs);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 以下はInterface向けのAddRemove
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>

    public bool AddObserver(IObserver<ModuleState> observer)
    {
       return base.AddObserver(observer);
    }

    public bool RemoveObserver(IObserver<ModuleState> observer)
    {
       return base.RemoveObserver(observer);
    }
}