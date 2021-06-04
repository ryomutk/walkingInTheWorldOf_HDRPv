using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建物の大きさの単位(何ブロック四方かとか)などを定義
/// </summary>
public class WorldSetting : Singleton<WorldSetting>
{
    public Vector3 SquairSize{get{return _SquareSize;}}
    [SerializeField]Vector3Int _SquareSize = new Vector3Int(9,9,9);
    
}
