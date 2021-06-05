using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
    {
        public static T instance = null;

        protected virtual void Awake()
        {
            CreateSingleton();
        }

        void CreateSingleton()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Debug.LogWarning(this.GetType() + "is singleton! now been killed");
                Destroy(this.gameObject);
            }
        }

        void OnDestroy()
        {
            instance = null;
        }

    }
}