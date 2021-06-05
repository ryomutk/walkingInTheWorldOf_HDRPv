using UnityEngine;

public class BuildBlockMediator : MediatorBase<BuildBlock>
{

    protected override bool ConvertNotice(BuildBlock source, IObserverBase observer)
    {
        if (!base.ConvertNotice(source, observer))
        {
            switch(observer)
            {
                case IObserver<ModuleState> obs1:
                obs1.OnNotice(source.state);
                return true;
            }
        }

        logger.LogError("Notice method for TYPE:"+observer.GetType() + "is not Implemented!");
        return false;
    }

}