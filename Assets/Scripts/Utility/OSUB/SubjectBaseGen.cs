using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectBehaviour<T> : MonoBehaviour,ISubject<T>
{
    List<IObserver<T>> observerList = new List<IObserver<T>>();

    public bool AddObserver(IObserver<T> observer){
        if(!observerList.Contains(observer)){
            observerList.Add(observer);
            return true;
        }
        return false;
    }

    public bool RemoveObserver(IObserver<T> observer){
        if(observerList.Contains(observer)){
            observerList.Remove(observer);
            return true;
        }

        return false;
    }

    public virtual bool Notice(T arg){
        
        foreach(var obs in observerList){
            obs.OnNotice(arg);
        }

        return true;
    }

    
}
