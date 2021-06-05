using UnityEngine;
using World.Interface.Geometry;

namespace World.Data.Geometry
{
    /// <summary>
    /// squareの幾何学情報だけを計算、保持するクラス。
    /// </summary>
    public class SquareMetaData : IRectangularData
    {
        public static Vector3Int gridSize { get { return _gridSize; } }
        static Vector3Int _gridSize = new Vector3Int(9, 9, 9);

        //グリッドサイズ。回転は適用しない→halfExtentを動かさないため
        public Vector3Int sizeGrid { get; private set; }

        /// <summary>
        /// 中心のワールド座標。回転を適用する
        /// </summary>
        /// <value></value>
        public Vector3 center { get { return originCenter + localCenter; } }

        /// <summary>
        /// ローカルな中心座標。originCenter基準。
        /// </summary>
        /// <value></value>
        public Vector3 localCenter { get { return rotation * _rawLocalCenter; } }
        public Vector3 halfExtent { get { return (Vector3)(sizeGrid * gridSize) / 2; } }

        public Vector3Int rotationEuler { get; private set; }
        public Quaternion rotation { get; private set; }

        /// <summary>
        /// 原点グリッドの中心座標.これがゲームオブジェクトの座標。
        /// </summary>
        public Vector3 originCenter { get; private set; }
        Vector3 _rawLocalCenter;

        public void MapSquare(ISquareObj sqrObj, Vector3Int originCoords, Vector3Int rotateEuler)
        {
            MapSquare(sqrObj.sizeGrid, originCoords, rotateEuler);
        }

        public void MapSquare(Vector3Int sizeGrid, Vector3Int originCoords, Vector3Int rotateEuler)
        {
            this.sizeGrid = sizeGrid;
            SetPosition(originCoords);
            SetRotation(rotateEuler);
        }

        public void SetPosition(Vector3Int originCoords)
        {
            var originLocalOrigin = -(gridSize - Vector3Int.one) / 2;
            originCenter = Vector3Int.Scale(originCoords, gridSize) - originLocalOrigin;


            _rawLocalCenter = Vector3.Scale((Vector3)gridSize / 2, sizeGrid - Vector3Int.one);
        }

        public void SetRotation(Vector3Int rotateEuler)
        {
            rotationEuler = rotateEuler;
            rotation = Quaternion.Euler(rotationEuler.x, rotationEuler.y, rotationEuler.z);
        }

    }
}