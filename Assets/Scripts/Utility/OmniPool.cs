using UnityEngine;
using Sirenix.OdinInspector;

/*
public class OmniPool : Singleton<OmniPool>
{
    public ObjPool<T> CreateNewPool<T>(T prefab, int num)
    where T : MonoBehaviour
    {
        if (ObjPool<T>.instance == null)
        {
            var pool = new ObjPool<T>(transform);
            pool.CreatePool(prefab, num);
            return pool;
        }

        return null;
    }


    public T GetObj<T>(bool activate = true)
    where T : MonoBehaviour
    {
        if (ObjPool<T>.instance == null)
        {
            Debug.LogWarning("ObjPoolOf" + typeof(T) + "is not created!");
            return null;
        }

        return ObjPool<T>.instance.GetObj(activate);
    }

    public bool CheckPool<T>()
    where T:MonoBehaviour
    {
        if(ObjPool<T>.instance == null){
            return false;
        }
        return true;
    }
}
*/