using UnityEngine;

public abstract class RoadBase : Structure, IRoad
{
    public Vector3Int path { get { return _path; } }
    [SerializeField] protected Vector3Int _path;

    //その道の曲がり属性。right,leftまたはnull(まっすぐな場合)が返ります
    public Direction? turn
    {
        get
        {
            if (path.x > 0)
            {
                return Direction.right;
            }
            else if (path.x < 0)
            {
                return Direction.left;
            }
            else
            {
                return null;
            }
        }
    }

    //その道の上がり下がり属性
    public Direction? stairs
    {
        get
        {
            if (path.y > 0)
            {
                return Direction.up;
            }
            else if (path.y < 0)
            {
                return Direction.down;
            }
            else
            {
                return null;
            }
        }
    }

    public abstract Vector3 GetPath();
}