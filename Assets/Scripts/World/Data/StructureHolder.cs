using UnityEngine;
using System.Collections.Generic;
using World.Building;



namespace World.Data
{
    [CreateAssetMenu]
    public class StructureHolder : ScriptableObject
    {
        [SerializeField] List<Structure> structureList;
        public int length { get { return structureList.Count; } }


        public Structure GetStructure(string name)
        {
            return structureList.Find(x => x.name == name);
        }

        public Structure GetStructure(int id)
        {
            return structureList[id];
        }

        public int GetID(Structure str)
        {
            return structureList.IndexOf(str);
        }
    }

    public enum StructureType
    {
        room
    }
}