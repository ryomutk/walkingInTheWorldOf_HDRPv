using UnityEngine;

public class PathFinder : SquareFinder
{
    private const int V = 0;
    Square _self = null;

    public PathFinder(Square self) : base()
    {
        _self = self;
    }

    public PathFinder():base(){}


    /// <summary>
    /// まだ直線しかみつけらんない
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="max"></param>
    /// <param name="vertical"></param>
    /// <returns></returns>
    public Doorway FindDoorway(Vector3Int direction, int max = 10, int vertical = 0)
    {
        if (_self == null)
        {
            Debug.LogError("self is null!");
            return null;
        }

        return FindDoorway(_self,direction,max,vertical);
    }

    public Doorway FindDoorway(Square originSquare, Vector3Int direction, int max = 10, int vertical = 0)
    {
        var coords = originSquare.coordinate;
        var square = StraightFind(coords, direction, max, originSquare.collider);
        var door = DetectDoor(square, direction);

        if (door == null && vertical != 0)
        {
            for (int i = 0; i < vertical; i++)
            {
                coords.y++;
                square = StraightFind(coords, direction, max, originSquare.collider);
                door = DetectDoor(square, direction);
                if (door != null)
                {
                    break;
                }
            }
        }

        return door;
    }

    Doorway DetectDoor(Square square, Vector3 direction)
    {
        if(square == null)
        {
            return null;
        }
        direction *= -1;
        var door = square.structure.doorwayList.Find(x => x.CheckAvalableDirection() == direction);

        return door;
    }
}