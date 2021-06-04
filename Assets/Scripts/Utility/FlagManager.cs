using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlagManager<T>
where T : Enum
{
    static T NowFlag;

    

    static public T AppendFlag(ref T flagBase, params T[] flags)
    {
        var nowFlagInt = (int)Convert.ChangeType(flagBase,typeof(int));
        foreach (int flag in flags as int[])
        {

            nowFlagInt |= flag;
        }

        flagBase = (T)Enum.ToObject(typeof(T),nowFlagInt);

        return (T)Enum.ToObject(typeof(T),nowFlagInt);
    }

    ///<summary>第一引数からそれに続く要素を消す。
    ///</summary>
    static public T RemoveFlag(ref T flagBase ,params T[] flags)
    {
        var nowFlagInt = (int)Convert.ChangeType(flagBase, typeof(int));
        for(int i = 0;i<flags.Length;i++){
            nowFlagInt &= ~(int)Convert.ChangeType(flags[i], typeof(int));
        }

        flagBase = (T)Enum.ToObject(typeof(T),nowFlagInt);

        return (T)Enum.ToObject(typeof(T),nowFlagInt);
    }

    
}