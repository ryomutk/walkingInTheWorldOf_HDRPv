using UnityEngine;
using Utility;

namespace World.Building
{
    public class Stairs : RoadBase
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
                return Vector3.up + Vector3.forward;
            }
            else if (doorwayList.Count == 2)
            {
                return doorwayList.Find(x => x.transform.localPosition != Vector3.zero).transform.localPosition + Vector3.forward + Vector3.up;
            }

            ErrorLogger.Log("FAILED TO GET PATH \n       DOOR COUNT: {0}", this.name);

            return Vector3.zero;

        }
    }
}

