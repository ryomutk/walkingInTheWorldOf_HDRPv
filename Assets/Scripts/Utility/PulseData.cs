using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
//null以外をdataに入れると、それが一フレームだけ読み取れるようになる。
//自家製triggerと同じようなものを、複数から読み取れるようにしたい場合に使う。
//厳格に一フレームだけ返されるのでそれを使っても良い。
///</summary>
public class PulseData<T>
where T : class
{
    bool pulse = false;

    T _data;

    int pulseLength = 1;

    public PulseData(int pLength=1){
        pulseLength = pLength;
    }

    public T data
    {
        get
        {
            if (pulse)
            {
                return _data;
            }
            else
            {
                return null;
            }
        }

        ///<summary>
        /// nullを投げればコルーチンは回りません。</summary>
        set
        {
            //セットしたら
            _data = value;
            //次の一フレームだけ読める"nullの場合はこのコルーチンを回さない"(結構重要)
            if (value != null)
            {
                Corutines.staticStarter(UniFramePulse());
            }
        }

    }

    IEnumerator UniFramePulse()
    {
        yield return null;
        pulse = true;
        //Debug.Log(typeof(T) +":"+ Time.frameCount);
        yield return pulseLength;
        //Debug.Log(typeof(T) +":"+Time.frameCount);
        pulse = false;
    }
}
