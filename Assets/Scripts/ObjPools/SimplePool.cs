using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//非MonoBehaviourで直下にPoolを作る
public class SimplePool<T>:IPoolObject<T>
where T:MonoBehaviour
{
    T objPrefab = null;
    ModuleState state;
    List<T> objList;
    Transform transform;

    public SimplePool(Transform trans) : base()
    {
        transform = trans;
        objList = new List<T>();
    }

    public bool CreatePool(T obj, int num)
    {
        if (num >= 0)
        {
            objPrefab = obj;
            state = ModuleState.working;

            for (int i = 0; i < num; i++)
            {
                CreateObj();
            }
        }
        return false;
    }


    public T GetObj(bool activate = true)
    {
        T returnObj = null;
        foreach (T obj in objList)
        {
            if (!obj.gameObject.activeSelf)
            {
                returnObj = obj;
                break;
            }
        }

        if (returnObj == null)
        {
            returnObj = CreateObj();
        }

        if (activate)
        {

            returnObj.gameObject.SetActive(true);
        }

        return returnObj;
    }

    T CreateObj()
    {
        if (state != ModuleState.disabled)
        {
            var clone = MonoBehaviour.Instantiate(objPrefab, transform);
            clone.transform.localPosition = Vector2.zero;
            objList.Add(clone);
            clone.gameObject.SetActive(false);
            return clone;
        }
        return null;
    }

    ~SimplePool()
    {
        MonoBehaviour.Destroy(transform);    
    }
}
