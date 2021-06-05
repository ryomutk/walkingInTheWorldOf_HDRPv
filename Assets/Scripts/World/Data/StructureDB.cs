using UnityEngine;
using World.Building;
using World.Data;
using Utility;

namespace World.Data.Database
{
    public class StructureDB : Singleton<StructureDB>
    {
        [SerializeField, Sirenix.OdinInspector.InlineEditor] StructureHolder holder;
        public int length { get { return holder.length; } }

        public Structure GetStructure(string name)
        {
            return holder.GetStructure(name);
        }

        public Structure GetStructure(int id)
        {
            return holder.GetStructure(id);
        }

        public int GetId(Structure structure)
        {
            return holder.GetID(structure);
        }
    }
}
