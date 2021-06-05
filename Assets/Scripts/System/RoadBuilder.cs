using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Utility;
using ModulePattern;
using Utility.ObjPool;

using World.Wrapper;
using World.Data.Building;


namespace World.Builder
{

    public class RoadBuilder : Singleton<RoadBuilder>
    {
        InstantPool<InterSquare> interSquarePool;
        [SerializeField] InterSquare interSquarePref;
        [SerializeField] int squarePoolNum = 500;
        public ModuleState state { get; private set; }

        Utility.Logger logger;

        void Start()
        {
            interSquarePool = new InstantPool<InterSquare>();
            logger = new Utility.Logger("RoadLog", "ROAD BUILDER");
            StartCoroutine(InstantiatePool());
        }

        IEnumerator InstantiatePool()
        {
            interSquarePool.CreatePool(interSquarePref, squarePoolNum);
            yield return new WaitUntil(() => interSquarePool.state == ModuleState.ready);
        }

        public void BuildRoad(RoadData data, Vector3 fromPosition, Vector3 forwardDirection)
        {
            BuildRoad(data, Vector3Int.RoundToInt(fromPosition), forwardDirection);
        }
        public void BuildRoad(RoadData data, Vector3Int fromPosition, Vector3 forwardDirection)
        {
            logger.Log("BUILD START!");
            StartCoroutine(BuildRoutine(data, fromPosition, forwardDirection));
        }

        //まだ座標を確認するフェーズがない！作って
        IEnumerator BuildRoutine(RoadData data, Vector3Int fromPosition, Vector3 forwardDirection)
        {
            //今回の前方をくおたーにおんに変形
            var forwardRotation = Quaternion.FromToRotation(Vector3.forward, forwardDirection);

            Vector3 nowPosition = fromPosition;
            for (int i = 0; i < data.count; i++)
            {
                var rotation = forwardRotation * data[i].Value;

                logger.LogLine("SET: on position:" + nowPosition.ToString().PadRight(20) + "path(modified):" + (rotation * data[i].Key.path).ToString().PadRight(20) + "rotation:" + data[i].Value.eulerAngles);

                var clone = Instantiate(data[i].Key);

                interSquarePool.GetObj().SetStructure(clone, Vector3Int.RoundToInt(nowPosition), rotation);
                nowPosition += data[i].Value * forwardRotation * data[i].Key.path;

                yield return 5;
            }

            logger.LogLine("ROAD BUILT!");
        }
    }
}