using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModulePattern;

namespace Utility.ObjPool
{

    /// <summary>
    /// GameobjectではなくModuleで管理する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModulePool<T>
    where T : MonoBehaviour, IModule
    {
        T objPrefab = null;
        ModuleState state;
        Transform parent;
        List<T> objList;
        Transform transform;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans">オブジェクトプールを作りたい場所のtransform</param>
        /// <returns></returns>
        public ModulePool(Transform trans) : base()
        {
            transform = trans;
            objList = new List<T>();
        }

        public bool CreatePool(T obj, int num)
        {
            if (num >= 0)
            {
                parent = new GameObject(obj.name + "Pool").transform;
                parent.SetParent(transform);
                objPrefab = obj;
                state = ModuleState.working;

                for (int i = 0; i < num; i++)
                {
                    CreateObj();
                }
            }

            return true;
        }


        public T GetObj()
        {
            T returnObj = null;
            foreach (T obj in objList)
            {
                if (obj.state == ModuleState.disabled)
                {
                    returnObj = obj;
                    break;
                }
            }

            if (returnObj == null)
            {
                returnObj = CreateObj();
            }

            return returnObj;
        }

        T CreateObj()
        {
            if (state != ModuleState.disabled)
            {
                var clone = MonoBehaviour.Instantiate(objPrefab, parent);
                clone.transform.localPosition = Vector2.zero;
                objList.Add(clone);
                return clone;
            }
            return null;
        }

        ~ModulePool()
        {
            MonoBehaviour.Destroy(parent);
        }
    }

}