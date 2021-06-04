using UnityEngine;

public class StandardBlock : BuildBlock
{
    public override BlockType type { get { return BlockType.standard; } }
    [SerializeField]public BlockBelongType belongs;
}

[System.Flags]
public enum BlockBelongType
{
    none = 0,
    floor = 1 << 0, //床または天井
    wall = 1 << 1   //壁,壁を動的に生成するときは、その上と下のブロックにもwall属性を付与するように！！
}