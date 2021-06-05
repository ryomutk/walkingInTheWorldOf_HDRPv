using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModulePattern;

namespace Utility.ObjPool
{
    //クラス単位での管理が不便な場合に使うObjPool。
    //同じクラスのプールを(prefab毎とかで)いくつでも作成できる代わりに、
    //singletonでのインターフェイスは確保できない(作成時に返される参照でしかアクセスできない。) 
    public class InstantPool<T> : IPoolObject<T>
    where T : MonoBehaviour
    {
        T objPrefab = null;
        public ModuleState state { get { return _state; } }
        ModuleState _state;
        Transform parent;
        List<T> objList;
        Transform transform;

        public InstantPool(Transform trans = null) : base()
        {
            transform = trans;
            objList = new List<T>();
        }

        public bool CreatePool(T obj, int num)
        {
            if (num >= 0)
            {
                parent = new GameObject(obj.name + "Pool").transform;
                if (transform != null)
                {
                    parent.SetParent(transform);
                }

                objPrefab = obj;
                _state = ModuleState.working;

                for (int i = 0; i < num; i++)
                {
                    CreateObj();
                }

                _state = ModuleState.ready;
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
            if (_state != ModuleState.disabled)
            {
                var clone = MonoBehaviour.Instantiate(objPrefab, parent);
                clone.name += objList.Count;
                clone.transform.localPosition = Vector2.zero;
                objList.Add(clone);
                clone.gameObject.SetActive(false);
                return clone;
            }
            return null;
        }

        ~InstantPool()
        {
            MonoBehaviour.Destroy(parent);
        }
    }

}