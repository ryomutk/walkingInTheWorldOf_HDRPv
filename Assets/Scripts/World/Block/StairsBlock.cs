using UnityEngine;
using System.Collections.Generic;


namespace World.Block
{
    public class StairsBlock : BuildBlock
    {
        public override BlockType type { get { return BlockType.stairs; } }
    }
}