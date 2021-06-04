using UnityEngine;
using Sirenix.OdinInspector;
public class SerializedSingleton<T> : SerializedMonoBehaviour
where T : SerializedSingleton<T>
{
    public static T instance = null;

    protected virtual void Awake()
    {
        CreateSingleton();   
    }

    void CreateSingleton(){
        if(instance == null){
            instance = this as T;
        }
        else{
            Debug.LogWarning( this.GetType() + "is singleton! now been killed");
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        instance = null;
    }


}