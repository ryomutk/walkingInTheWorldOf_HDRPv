using UnityEngine;
using World.Rader;

namespace ObserverPattern
{
    public class DetectorMediator<T> : MediatorBase<DetectorBehaviour<T>>
    {
        protected override bool ConvertNotice(DetectorBehaviour<T> source, IObserverBase observer)
        {
            switch (observer)
            {
                case IObserver<GameObject> obs1:
                    obs1.OnNotice(source.gameObject);
                    return true;
            }

            if (simpleNoticedList.Contains(observer))
            {
                observerList.Remove(observer);
                logger.Log("!ATTENTION! OBSERVER" + observer + "IS NOW ONLY FOR SIMPLE");
                return true;
            }

            logger.LogError("Notice method for TYPE:" + observer.GetType() + "is not Implemented!");

            return false;
        }
    }

}