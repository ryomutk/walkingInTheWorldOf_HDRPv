using UnityEngine;

[CreateAssetMenu]
public class SimpleRoadSet : RoadSetBase, IRoadSet
{
    public override RoadBase turnRight
    {
        get
        {
            return _turnRight;
        }
    }
    public override RoadBase turnLeft
    {
        get
        {
            return _turnLeft;
        }
    }


    [SerializeField] RoadBase _corriderSlice;
    [SerializeField] RoadBase _turnRight;
    [SerializeField] RoadBase _turnLeft;
    [SerializeField] RoadBase _stairsSlice;

    public override RoadBase GetSlice(RoadType type)
    {
        if(type == RoadType.corridor)
        {
            return _corriderSlice;
        }
        else if(type == RoadType.stairs)
        {
            return _stairsSlice;
        }
        else
        {
            logger.LogError("ERROR: UNKNOWN TYPE!");
            return null;
        }
    }

    protected override bool Authorize()
    {
        base.Authorize();

        //スライス最小単位なので一マス単位である必要がある
        if (_corriderSlice.turn != null || _corriderSlice.path.z != 1)
        {
            logger.LogError("!ERROR! check corridor slice");
            return false;
        }

        if (!(Mathf.Abs(_stairsSlice.path.y) == 1) || _stairsSlice.path.y != 1 || _stairsSlice.turn != null)
        {
            logger.LogError("!ERROR! stairs slice");

            return false;
        }

        return true;
    }
}