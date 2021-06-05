using UnityEngine;
using World.Building;

namespace World.Data.Prefab
{
    //道路作成に必要な道のセット
    public interface IRoadSet
    {
        RoadBase turnRight { get; }
        RoadBase turnLeft { get; }


        RoadBase GetSlice(RoadType type);
    }

    public enum RoadType
    {
        corridor,
        stairs
    }
}