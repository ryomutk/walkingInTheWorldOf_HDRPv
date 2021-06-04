using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NullReferenceChecker
{
    public static bool HasBrokenObj<T>(List<T> list)
    where T : MonoBehaviour
    {
        try
        {
            list.Any(x => x.gameObject);
        }
        catch (MissingReferenceException)
        {
            return true;
        }


        return false;
    }

    public static bool HasNullObj<T>(params T[] objs)
    where T : MonoBehaviour
    {
        try
        {
            foreach (T obj in objs)
            {
                var a = obj.gameObject;
            }

        }
        catch(NullReferenceException)
        {
            return true;
        }

        return false;
    }

    public static bool HasBrokenObj<T>(T obj)
    where T : MonoBehaviour
    {
        try
        {
            var a = obj.gameObject;
        }
        catch
        {
            return true;
        }
        return false;
    }
}
