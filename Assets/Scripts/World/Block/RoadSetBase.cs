using UnityEngine;
using World.Building;

namespace World.Data.Prefab
{

    public abstract class RoadSetBase : ScriptableObject, IRoadSet
    {
        public abstract RoadBase turnRight { get; }
        public abstract RoadBase turnLeft { get; }
        protected Utility.Logger logger;


        void OnValidate()
        {
            Authorize();
            logger = new Utility.Logger("RoadLog", this.name);
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
}