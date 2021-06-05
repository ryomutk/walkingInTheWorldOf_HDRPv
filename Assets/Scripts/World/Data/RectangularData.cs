using UnityEngine;
using World.Building;

namespace World.Data.Geometry
{
    //SquareにフィットしていないタイプのRectangularData
    public class RectangularData : IRectangularData
    {
        public Vector3Int rotationEuler { get { return Vector3Int.RoundToInt(rotation.eulerAngles); } }
        public Vector3 center { get { return transPosition + localCenter; } }
        public Vector3 halfExtent { get; private set; }
        public Quaternion rotation { get; private set; }

        /// <summary>
        /// centerのtransform.positionからのずれ。
        /// </summary>
        /// <value></value>
        public Vector3 localCenter { get; private set; }

        /// <summary>
        /// 世界座標での位置
        /// </summary>
        /// <value></value>
        public Vector3 transPosition { get; private set; }



        public RectangularData MapRectangular(Structure structure, Vector3Int originCoords, Quaternion rotation)
        {
            return this.SetStructure(structure)
            .SetPosition(originCoords)
            .SetRotation(rotation);
        }

        public RectangularData SetStructure(Structure structure)
        {
            localCenter = structure.localCenter;
            halfExtent = (Vector3)structure.size / 2;

            return this;
        }

        public RectangularData SetPosition(Vector3Int originCoords)
        {
            transPosition = originCoords;
            return this;
        }

        public RectangularData SetRotation(Quaternion rotate)
        {
            rotation = rotate;
            return this;
        }
    }
}

