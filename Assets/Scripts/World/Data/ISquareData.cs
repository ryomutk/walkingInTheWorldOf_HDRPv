using UnityEngine;

namespace World.Data.Geometry
{
    public interface IRectangularData
    {
        Vector3Int rotationEuler { get; }
        Vector3 center { get; }
        Vector3 halfExtent { get; }
        Quaternion rotation { get; }
    }
}