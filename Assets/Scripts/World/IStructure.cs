using UnityEngine;
using System.Collections.Generic;
using World.Block;
using World.Path;

namespace World.Interface.Building
{
    /// <summary>
    /// 構造物のアレコレに必要なアレコレ
    /// </summary>
    public interface IStructure
    {
        string name { get; }
        Vector3 size { get; }
        Vector3 peak { get; }
        List<Doorway> avalableDoorwayList { get; }
        List<Doorway> doorwayList { get; }
        List<BuildBlock> blockList { get; }
    }

    public enum StructureState
    {
        none,
        Building,   //建設中(まだ実体がない)
        Built       //建設済み
    }
}