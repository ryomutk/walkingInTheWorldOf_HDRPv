using UnityEngine;
using Sirenix.OdinInspector;

public abstract class SerializedScriptableSingleton<T>:SerializedScriptableObject
where T:SerializedScriptableSingleton<T>
{
    
    static T instance = null;

    protected virtual void OnEnable()
    {
        CreateSingleton();
    }

    void CreateSingleton(){

        if(instance == null){
            instance = this as T;
            Debug.Log(typeof(T) + "now avalable on singleton");
        }
        else{
            Debug.LogWarning( this.GetType() + "is singleton! now been killed");
            Destroy(this);
        }
    }

}


