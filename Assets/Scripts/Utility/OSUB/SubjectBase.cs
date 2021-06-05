using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{

    public class SubjectBehaviour : MonoBehaviour, ISubject
    {
        List<IObserver> observerList = new List<IObserver>();

        public virtual bool AddObserver(IObserver observer)
        {
            if (!observerList.Contains(observer))
            {
                observerList.Add(observer);
                return true;
            }
            return false;
        }

        public virtual bool RemoveObserver(IObserver observer)
        {
            if (observerList.Contains(observer))
            {
                observerList.Remove(observer);
                return true;
            }

            return false;
        }

        /// <summary>
        /// どれか一つでも失敗したらfalseを返す
        /// </summary>
        /// <returns></returns>
        public virtual bool Notice()
        {
            bool result = true;
            foreach (var obs in observerList)
            {
                if (!obs.OnNotice())
                {
                    result = false;
                }
            }

            return result;
        }
    }
}