using UnityEngine;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace World.Math
{
    public class MathOfWorld
    {
        static public ReadOnlyCollection<Vector3Int> rotateEulers { get { return Array.AsReadOnly(_eulerSets); } }
        static public ReadOnlyCollection<Vector3Int> directions { get { return Array.AsReadOnly(_directions); } }

        /// <summary>
        /// 一マスのサイズ
        /// </summary>
        /// <returns></returns>
        static public Vector3Int gridSize = new Vector3Int(9, 9, 9);

        //readonlyにしたいンゴねぇ
        public Dictionary<Direction, Vector3Int> dirDic { get { return _dirDic; } }


        static Vector3Int[] _directions = new Vector3Int[6]
        {
        new Vector3Int(0,0,1),
        Vector3Int.right,
        Vector3Int.left,
        new Vector3Int(0,0,-1),
        Vector3Int.up,
        Vector3Int.down
        };
        static Dictionary<Direction, Vector3Int> _dirDic = new Dictionary<Direction, Vector3Int>()
    {
        {Direction.forward,_directions[0]},
        {Direction.right,_directions[1]},
        {Direction.left,_directions[2]},
        {Direction.back,_directions[3]},
        {Direction.up,_directions[4]},
        {Direction.down,_directions[5]}
    };

        public static Dictionary<Vector3Int, Direction> _dirDicRev = new Dictionary<Vector3Int, Direction>
    {
        {_directions[0],Direction.forward},
        {_directions[1],Direction.right},
        {_directions[2],Direction.left},
        {_directions[3],Direction.back},
        {_directions[4],Direction.up},
        {_directions[5],Direction.down}
    };


        static Vector3Int[] _eulerSets = new Vector3Int[4]
        {
        new Vector3Int(0,0,0),
        new Vector3Int(0,90,0),
        new Vector3Int(0,180,0),
        new Vector3Int(0,270,0)
        };

        /// <summary>
        /// ワールド座標から立体グリッド座標に変換
        /// </summary>
        /// <returns></returns>
        public static Vector3Int PositionToCoordinate(Vector3 position)
        {
            var posint = Vector3Int.CeilToInt(position);
            posint.x /= gridSize.x;
            posint.y /= gridSize.y;
            posint.z /= gridSize.z;

            return posint;
        }

        static public Direction GetDirection(Vector3 directionVector)
        {
            var directionVecInt = Vector3Int.RoundToInt(directionVector);
            if (directionVecInt.sqrMagnitude > 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (directionVecInt[i] != 0)
                    {
                        directionVecInt[i] /= Mathf.Abs(directionVecInt[i]);
                    }
                }

            }
            return _dirDicRev[directionVecInt];
        }
    }
}