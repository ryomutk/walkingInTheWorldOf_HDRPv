using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    //シーンが始まる前に初期化しなきゃいけないクラスにつける
    public interface IInitialize
    {
        bool initialized { get; }
    }

}