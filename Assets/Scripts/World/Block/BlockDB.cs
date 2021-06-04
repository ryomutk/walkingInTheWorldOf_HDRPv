using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockDB : Singleton<BlockDB>
{
    [SerializeField,Sirenix.OdinInspector.InlineEditor] List<BlockHolder> _holders;


    public BuildBlock Get(BlockType type, int index = 0)
    {
        var holder = _holders.Find(x => x.type == type);

        if (holder == null)
        {
            Debug.LogWarning("block of type" + type + "is not set!");
        }

        return holder.GetBlock(index);
    }
}
