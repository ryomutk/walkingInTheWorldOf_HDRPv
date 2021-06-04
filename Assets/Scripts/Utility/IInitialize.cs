using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シーンが始まる前に初期化しなきゃいけないクラスにつける
public interface IInitialize
{
    bool initialized{get;}
}
