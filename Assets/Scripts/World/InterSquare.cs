using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Building;
using World.Data.Geometry;

namespace World.Wrapper
{
    /// <summary>
    /// 階段や廊下、間を埋める壁などの中間構造体
    /// </summary>
    public class InterSquare : MonoBehaviour
    {
        protected Structure entity;
        public RectangularData recData { get; private set; }

        new BoxCollider collider;

        protected virtual void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            recData = new RectangularData();
            collider = GetComponent<BoxCollider>();
        }

        [Sirenix.OdinInspector.Button]
        public void SetStructure(Structure structure, Vector3Int position, Quaternion rotation)
        {
            Initialize();

            recData.MapRectangular(structure, position, rotation);
            collider.size = recData.halfExtent * 2 - Vector3.one * 0.1f;
            collider.center = recData.localCenter;

            structure.transform.SetParent(transform);
            structure.transform.localPosition = Vector3.zero;

            transform.position = recData.transPosition;
            transform.rotation = recData.rotation;
        }
    }

}
