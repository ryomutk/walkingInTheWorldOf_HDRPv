using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Squareたちの位置情報に関する処理のクラス。
/// </summary>
public class SquareMapper: Singleton<SquareMapper>
{
    //public Dictionary<Vector3Int,Square> worldMap { get { return  _worldMap;} }
    Dictionary<Vector3Int, Square> _worldMap = new Dictionary<Vector3Int, Square>();
    SquareFinder finder;

    void Start()
    {
        finder = new SquareFinder();
    }

    //新しいsquareをその原点座標に登録する。
    public bool AddSquare(Square square)
    {
        _worldMap.Add(square.coordinate, square);
        return true;
    }

    public bool CheckCoordinate(Vector3Int coordinate)
    {
        if (!_worldMap.ContainsKey(coordinate))
        {
            var sqr = finder.Cast(coordinate);
            if (sqr != null)
            {
                _worldMap[coordinate] = sqr.GetComponent<Square>();
                return true;
            }
            return false;
        }

        return true;
    }

    public bool CheckCoordinate(Vector3Int coordinate, out Square hit)
    {
        LogWriter.Log("SQUARE MAPPER CHECK COORDINATE","CastingLog");
        if (!_worldMap.ContainsKey(coordinate))
        {
            var col = finder.Cast(coordinate);
            if (col != null)
            {
                var sqr = col.GetComponent<Square>();
                _worldMap[coordinate] = sqr;
                hit = sqr;
                return true;
            }
            hit = null;
            return false;
        }

        hit = _worldMap[coordinate];
        return true;
    }

    //SquareFinderの同名メソッドへのインターフェイス
    public bool CheckSquare(SquareMetaData metaSquare)
    {
        return finder.CheckSquare(metaSquare);
    }

    /// <summary>
    /// 登録されている情報を全部消す。
    /// </summary>
    /// <param name="square"></param>
    public void RemoveSquare(Square square)
    {
        var removes = _worldMap.Where(x => x.Value == square);

        foreach (var keyValues in removes)
        {
            _worldMap.Remove(keyValues.Key);
        }
    }
}