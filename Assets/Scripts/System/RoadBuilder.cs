using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class RoadBuilder : Singleton<RoadBuilder>
{
    InstantPool<InterSquare> interSquarePool;
    [SerializeField] InterSquare interSquarePref;
    [SerializeField] int squarePoolNum = 500;
    public ModuleState state { get; private set; }

    Logger logger;

    void Start()
    {
        interSquarePool = new InstantPool<InterSquare>();
        logger = new Logger("RoadLog", "ROAD BUILDER");
        StartCoroutine(InstantiatePool());
    }

    IEnumerator InstantiatePool()
    {
        interSquarePool.CreatePool(interSquarePref, squarePoolNum);
        yield return new WaitUntil(() => interSquarePool.state == ModuleState.ready);
    }

    public void BuildRoad(RoadData data, Vector3 fromPosition, Quaternion forwardRotation)
    {
        BuildRoad(data,Vector3Int.RoundToInt(fromPosition),forwardRotation);
    }
    public void BuildRoad(RoadData data, Vector3Int fromPosition, Quaternion forwardRotation)
    {
        logger.Log("BUILD START!");
        StartCoroutine(BuildRoutine(data, fromPosition, forwardRotation));
    }

    //まだ座標を確認するフェーズがない！作って
    IEnumerator BuildRoutine(RoadData data, Vector3Int fromPosition, Quaternion forwardRotation)
    {
        Vector3 nowPosition = fromPosition;
        for (int i = 0; i < data.count; i++)
        {
            var rotation = forwardRotation * data[i].Value;

            logger.LogLine("SET: of path:" + data[i].Key.path + "rotation:" + data[i].Value.eulerAngles);

            interSquarePool.GetObj().SetStructure(data[i].Key, Vector3Int.RoundToInt(nowPosition), rotation);
            nowPosition += data[i].Value * forwardRotation * data[i].Key.path;

            yield return 5;
        }

        logger.LogLine("ROAD BUILT!");
    }
}