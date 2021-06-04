using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoadServer : Singleton<RoadServer>
{
    [SerializeField] RoadSetBase _defaultRoadSet;
    [SerializeField] RoadType[] defaultPathPattern = new RoadType[3] { RoadType.stairs, RoadType.corridor, RoadType.stairs };
    Logger logger = new Logger("RoadLog", "ROAD SERVER");


    public RoadData GetRoadsByPath(Vector3 path, Direction endDirection, RoadType[] pathPattern = null)
    {
        return GetRoadsByPath(Vector3Int.RoundToInt(path),endDirection,pathPattern);
    }
    public RoadData GetRoadsByPath(Vector3Int path, Direction endDirection, RoadType[] pathPattern = null)
    {
        return GetRoadsByPath(_defaultRoadSet, path, endDirection, pathPattern);
    }

    public RoadData GetRoadsByPath(IRoadSet roadSet, Vector3Int path,Direction endDirection, RoadType[] pathPattern = null)
    {
        if(pathPattern == null)
        {
            pathPattern = defaultPathPattern;
        }

        var data = new RoadData();

        //計算で使う、変数としてのpath
        var undefinedPath = path;
        int turnCount = 0;

        logger.Log("START: creating path set");
        logger.LogLine("PATH:" + path);

        if (SteapCheck(path))
        {
            logger.LogLine("FAILED: too steap");
            return null;
        }

        //曲がり角をつけれるならば
        if (path.x > roadSet.turnRight.path.x)
        {
            //目的地が前なら
            if (endDirection == Direction.forward)
            {
                //一回右に曲がり、その後左に曲がり、ベクトルをもとに戻す
                data.Add(roadSet.turnRight);
                data.Add(roadSet.turnLeft);

                turnCount = 2;
            }
            //目的地が右なら
            else if (endDirection == Direction.right)
            {
                //右折を一つまみ
                data.Add(roadSet.turnRight);
                turnCount = 1;
            }
            else
            {
                logger.LogError("ERROR!:end direction" + endDirection + " is not suitable",true);
                return null;
            }
        }
        //左折も同じ
        else if (path.x < roadSet.turnLeft.path.x)
        {
            if (endDirection == Direction.forward)
            {
                data.Add(roadSet.turnLeft);
                data.Add(roadSet.turnRight);

                turnCount = 2;
            }
            else if (endDirection == Direction.left)
            {
                data.Add(roadSet.turnLeft);
                turnCount = 1;
            }
            else
            {
                logger.LogError("ERROR!:end direction" + endDirection + " is not suitable",true);
                return null;
            }
        }

        if (turnCount != 0)
        {
            //未確定Path数を修正
            undefinedPath += data.GetNowPath();

            if (path.x * undefinedPath.x < 0 || path.y * undefinedPath.x < 0)
            {
                logger.LogLine("FAILED: path was too short to have turn");
                return null;
            }
            logger.LogLine("REPORT: turns set! undefined path is now" + undefinedPath);
        }

        //残った分のpathが急すぎないか確認
        if (!SteapCheck(undefinedPath))
        {
            logger.LogLine("FAILED: undefined path is too steap");
        }

        //必要な階段、廊下（スライス）の数。
        int corridorNum = 0;
        int stairsNum = 0;

        //それはそれぞれ、未確定のpath のy成分と、x,z成分の合計から階段分を引いたものに等しい
        stairsNum = Mathf.Abs(undefinedPath.y);
        corridorNum = Mathf.Abs(undefinedPath.z) + Mathf.Abs(undefinedPath.x) - stairsNum;

        //各セクションのかず
        int corridorSCount = pathPattern.Count(x => x == RoadType.corridor);
        int stairsSCount = pathPattern.Count(x => x == RoadType.stairs);



        //各セクションにある廊下、階段の数
        int unitCorridorNum = corridorNum / corridorSCount;
        int unitStairsNum = stairsNum / stairsSCount;

        //数の端数。
        int surplusCorridor = corridorNum % corridorSCount;
        int surplusStairs = stairsNum % stairsSCount;

        //道のパート(何回曲がったか)
        int nowRoadPart = 0;

        //一番最初の直進道の個数。
        int alterStraightNum = 0;
        //曲がった後の直真道の個数。（一つ以上にはならない）turnCount = 0の場合は使われない。
        int turnedRoadNum = 0;

        if (turnCount == 2)
        {
            //二回曲がる場合、直進が二つに分かれる
            alterStraightNum = undefinedPath.z / 2;
        }
        else
        {
            alterStraightNum = undefinedPath.z;
        }

        turnedRoadNum = undefinedPath.x;

        foreach (var type in pathPattern)
        {
            int tmpSliceNum = 0;

            if (type == RoadType.corridor)
            {
                tmpSliceNum = unitCorridorNum;
                if (surplusCorridor != 0)
                {
                    surplusCorridor--;
                    tmpSliceNum += 1;
                }
            }
            else if (type == RoadType.corridor)
            {
                tmpSliceNum = unitStairsNum;
                if (surplusCorridor != 0)
                {
                    surplusStairs--;
                    tmpSliceNum++;
                }

            }
            else
            {
                logger.LogError("ERROR:something went wrong in here",true);
                return null;
            }


            //廊下/会談をunit数だけ並べる
            for (int i = 0; i < tmpSliceNum; i++)
            {
                //GetSliceはせっかくメソッドなので毎回呼ぶことにする。
                var slice = roadSet.GetSlice(type);

                //一回も曲がらないならただ並べればよい
                if (turnCount == 0)
                {
                    data.Add(slice);
                }
                else
                {
                    //今登録されている道のうち、まだ通っていないTurnを飛ばして一番後ろにInsert
                    data.Insert(data.count - turnCount + nowRoadPart, slice);

                    //これは一番単純な道を作るあれなので、nowRoadPartが2より大きくなることはない
                    if (nowRoadPart == 0)
                    {
                        alterStraightNum--;
                        if (alterStraightNum == 0)
                        {
                            nowRoadPart++;
                        }
                    }
                    else if (nowRoadPart == 1)
                    {
                        turnedRoadNum--;
                        if (alterStraightNum == 0)
                        {
                            nowRoadPart++;
                        }
                    }
                }
                /*
                else
                {
                    logger.LogLine("ERROR:unknown turn count");
                    return null;
                }
                */
            }
        }

        undefinedPath -= data.GetNowPath();

        //undefinedPathは正常でも直進の場合のみ到着の向きに直交する部分の座標がずれる場合がある
        logger.LogLine("ROAD CREATION COMPLEATED WITH undefinedPath:" + undefinedPath);

        return data;
    }

    bool SteapCheck(Vector3Int path)
    {
        return Mathf.Abs(path.x) + Mathf.Abs(path.z) > Mathf.Abs(path.y);
    }

}



public enum TrimType
{
    normalCut,  //turnの前後を切る。
    endCut,     //後ろを切る
    startCut,   //前を切る
}

public enum RoadType
{
    corridor,
    stairs
}