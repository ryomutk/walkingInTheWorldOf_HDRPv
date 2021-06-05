using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace World.Building
{
    //廊下。doorway
    //目の前のブロックに0,0をつなげる想定。
    public class Corridor : RoadBase
    {

        public override void Remap()
        {
            base.Remap();
            _path = Vector3Int.RoundToInt(GetPath());
        }

        public override Vector3 GetPath()
        {
            if (doorwayList.Count == 1)
            {
                return Vector3.forward;
            }
            else if (doorwayList.Count == 2)
            {
                return doorwayList.Find(x => x.transform.localPosition != Vector3.zero).transform.localPosition + Vector3.forward;
            }

            ErrorLogger.Log("FAILED TO GET PATH \n       DOOR COUNT: {0}", this.name);

            return Vector3.zero;
        }
    }
}
