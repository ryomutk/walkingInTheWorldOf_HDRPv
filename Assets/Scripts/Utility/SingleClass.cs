using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClass<T>
where T:SingleClass<T>
{
    public static T instance{get{return _instance;} private set{_instance = value;}}
    static T _instance = null;

    protected SingleClass()
    {
        CreateSingleton();   
    }

    void CreateSingleton(){
        if(instance == null){
            instance = this as T;
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    ~SingleClass(){
        instance = null;
    }
}
