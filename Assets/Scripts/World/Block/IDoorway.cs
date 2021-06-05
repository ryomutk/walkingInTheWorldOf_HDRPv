using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Path
{
    /// <summary>
    /// 出入口を示すためのブロック。このブロック同士をくっつけることで建築物をくっつけるという発想。
    ///  directionの向きに別の構造物のdoorwayが来ると想定してください。
    /// このブロックさえ把握していれば建造物の位置関係の管理ができるのが理想
    /// </summary>
    public interface IDoorway
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CheckConnection();
    }
}