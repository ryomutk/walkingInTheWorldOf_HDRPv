using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoadData
{
    Logger logger = new Logger("RoadLog", "ROAD DATA");

    List<RoadBase> roads = new List<RoadBase>();
    List<Quaternion> _rotationList = new List<Quaternion>();
    List<Vector3Int> _modifiedPath = new List<Vector3Int>();
    bool isDarty = false;
    public int count { get { return roads.Count; } }

    int turnCount;

    public KeyValuePair<RoadBase, Quaternion> this[int index]
    {
        get
        {
            if (isDarty)
            {
                CleanUp();
            }

            return new KeyValuePair<RoadBase, Quaternion>(roads[index], _rotationList[index]);
        }
    }

    public Vector3Int GetNowPath()
    {
        CleanUp();
        var returnPath = new Vector3Int();

        foreach(var path in _modifiedPath)
        {
            returnPath += path;
        }
        logger.Log("now modpath is" + returnPath);
        return returnPath;
    }

    /// <summary>
    /// DartyになったPathをrotationEulerに適用し、ModifiedPath持つ来る
    /// </summary>
    void CleanUp()
    {
        _rotationList.Clear();
        
        var nowRotation = Vector3Int.zero;
        foreach (var road in roads)
        {
            _rotationList.Add(Quaternion.Euler(nowRotation));

            if (road.turn != null)
            {
                if (road.turn == Direction.right)
                {
                    nowRotation.y += 90;
                }
                else if (road.turn == Direction.left)
                {
                    nowRotation.y -= 90;
                }
            }
        }

        ModifyPath();

        isDarty = false;
    }

    void ModifyPath()
    {
        _modifiedPath.Clear();
        for (int i = 0; i < _rotationList.Count; i++)
        {
            var mpasf = _rotationList[i] * roads[i].path;
            var mpas = Vector3Int.RoundToInt(mpasf);
            _modifiedPath.Add(mpas);
        }
    }

    ///
    //　以下基本メソッド
    /// 

    public void Add(RoadBase road)
    {
        isDarty = true;

        logger.LogLine("Added road of path"+road.path);

        roads.Add(road);
    }

    public void Remove(int? count = null)
    {
        isDarty = true;

        if (count == null)
        {
            count = roads.Count;
        }

        logger.LogLine("Removed Road of rawPath;" + roads[count.Value].path);

        roads.RemoveAt(roads.Count);
    }

    public void Insert(int index,RoadBase road)
    {
        isDarty = true;

        logger.LogLine("Inserted road of path" + road.path + " in index:" + index);

        roads.Insert(index,road);
    }

    public void Relpace(RoadBase road, int index)
    {
        isDarty = true;

        roads.RemoveAt(index);
        roads.Insert(index, road);
    }
    /*

    ///
    //  以下Trim用メソッド
    //   Trim いらなくなりそう（笑）
    /// 

    //指定されたpathに適合するように廊下(turnでない)をトリミング
    public bool Trim(Vector3Int targetPath, TrimType type = TrimType.normalCut)
    {
        logger.LogLine("---TRIM START---");
        if (isDarty)
        {
            CleanUp();
        }

        var nowPath = Vector3Int.zero;

        foreach (var elem in _modifiedPath)
        {
            nowPath += elem;
        }

        var differ = nowPath - targetPath;

        if (differ.y != 0)
        {
            logger.LogLine("ERROR: Too many stairs?");
            return false;
        }

        return OperateTrim(differ, type);
    }

    bool OperateTrim(Vector3Int differ, TrimType type)
    {
        if (type == TrimType.endCut)
        {
            return EndCut(differ);
        }
        else if (type == TrimType.normalCut)
        {
            return NormalCut(differ);
        }
        else if (type == TrimType.startCut)
        {
            return StartCut(differ);
        }

        logger.LogLine("TRIM ERROR: Unknown type");
        return false;
    }

    //曲がり角毎にその分の廊下を取り去る。
    bool NormalCut(Vector3Int differ)
    {
        throw new System.NotImplementedException();

        //曲がり角の実態とそのインデックスの列挙を取得
        var turnParts = roads.Select((r, i) => new { road = r, index = i })
        .Where(cls => cls.road.turn != null);


        //曲がり角分を計算し、differと適合するか確認
        foreach (var elm in turnParts)
        {

        }

        for (int i = 0; i < turnParts.Count(); i++)
        {
            var targetIndex = turnParts.ElementAt(i).index;
            int endIndex;

            if (i != turnParts.Count() - 1)
            {
                endIndex = turnParts.ElementAt(i + 1).index;
            }
            else
            {
                endIndex = roads.Count - 1;
            }

            for (int t = targetIndex + 1; t < endIndex; t++)
            {
                throw new System.NotImplementedException();
            }

        }
    }

    bool EndCut(Vector3Int differ)
    {
        Vector2Int difSign = Vector2Int.one;

        if (differ.x < 0)
        {
            difSign.x = -1;
        }
        if (differ.z < 0)
        {
            difSign.y = -1;
        }

        Vector3Int absdif = new Vector3Int();
        System.Func<Vector3Int> absDiffer = () =>
        {
            absdif.x = differ.x * difSign.x;
            absdif.y = differ.z * difSign.y;
            return absdif;
        };


        for (int i = count - 1; i >= 0; i--)
        {
            if (roads[i].turn == null && roads[i].stairs == null)
            {
                differ -= _modifiedPath[i];

                //切りすぎてしまった場合
                if (absDiffer().x < 0 || absDiffer().z < 0)
                {
                    //戻してRemoveは実行せずに続ける
                    differ += _modifiedPath[i];
                    continue;
                }

                //切りすぎてなければRemove。Endからなのでそのまま切ってもインデックスはずれない。
                Remove(i);

                //ジャストの場合
                if (absDiffer().x == 0 && absDiffer().y == 0)
                {
                    logger.LogLine("END CUT SUCCESS!");
                    //Roadだけ減ってしまっているのでマップしなおす
                    CleanUp();
                    return true;
                }

            }
        }


        //全部やってもかえってなければ失敗で終了
        logger.LogLine("END CUT FAILED");
        return false;
    }

    bool StartCut(Vector3Int differ)
    {
        Vector2Int difSign = Vector2Int.one;

        if (differ.x < 0)
        {
            difSign.x = -1;
        }
        if (differ.z < 0)
        {
            difSign.y = -1;
        }

        Vector3Int absdif = new Vector3Int();
        System.Func<Vector3Int> absDiffer = () =>
        {
            absdif.x = differ.x * difSign.x;
            absdif.y = differ.z * difSign.y;
            return absdif;
        };
        List<int> cutIndex = new List<int>();

        for (int i = 0; i < count; i++)
        {
            if (roads[i].turn == null && roads[i].stairs == null)
            {
                differ -= _modifiedPath[i];

                //切りすぎてしまった場合
                if (absDiffer().x < 0 || absDiffer().z < 0)
                {
                    //戻してRemoveは実行せずに続ける
                    differ += _modifiedPath[i];
                    continue;
                }

                //前から切るとIndexがずれるので切るリストにいったん保存
                cutIndex.Add(i);

                //ジャストの場合
                if (absDiffer().x == 0 && absDiffer().y == 0)
                {
                    break;
                }
            }
        }

        //切り切れてなかったら
        if (differ.x != 0 || differ.y != 0)
        {
            //失敗で終了
            logger.LogLine("START CUT FAILED");
            return false;
        }

        //インデックスが後ろのほうから切る。
        for (int i = cutIndex.Count - 1; i >= 0; i--)
        {
            Remove(cutIndex[i]);
        }


        //Roadだけ減ってしまっているのでマップしなおす
        CleanUp();

        logger.LogLine("START CUT SUCCESS!");
        //成功で終了
        return true;
    }
    */
}