using UnityEngine;

public abstract class RoadSetBase : ScriptableObject,IRoadSet
{
    public abstract RoadBase turnRight { get; }
    public abstract RoadBase turnLeft { get; }
    protected Logger logger;


    void OnValidate()
    {
        Authorize();
        logger = new Logger("RoadLog", this.name);
        logger.Log("Resources authorized!");
    }
    public abstract RoadBase GetSlice(RoadType type);

    protected virtual bool Authorize()
    {

        //曲がり角は正しく曲がってさえいれば良い
        if (turnRight.turn != Direction.right)
        {
            logger.LogError("!ERROR! check right part");

            return false;
        }

        if (turnLeft.turn != Direction.left)
        {
            logger.LogError("!ERROR! check left part");

            return false;
        }



        return true;
    }
}