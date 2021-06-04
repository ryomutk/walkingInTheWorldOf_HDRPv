using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface便利クラス
/// </summary>
public class InterfaceUtils
{
    /// <summary>
    /// 特定のインタフェースがアタッチされたオブジェクトを見つける
    /// </summary>
    /// <typeparam name="T"> 探索するインタフェース </typeparam>
    /// <returns> 取得したクラス配列 </returns>
    public static T[] FindObjectOfInterfaces<T>() where T : class
    {
        List<T> findList = new List<T>();

        // オブジェクトを探索し、リストに格納
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            findList.Add(obj);
        }

        
        T[] findObjArray = new T[findList.Count];
        int count = 0;

        // 取得したオブジェクトを指定したインタフェース型配列に格納
        foreach (T obj in findList)
        {
            findObjArray[count] = obj;
            count++;
        }

        return findObjArray;
    }
}