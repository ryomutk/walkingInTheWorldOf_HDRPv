using UnityEngine;
using InputSystem;
using ObserverPattern;
using Utility.UI;

using World.Wrapper;
using World.Rader.Core; 
using World.Path;
using World.Map;
using World.Math;
using World.Builder;
using World.Data.Database;



namespace World.Testor.Rader
{

    //doorwayを踏んだらとりあえず空いてる向きを走査するやつを作る
    public class CasterTestor : MonoBehaviour, IObserver
    {
        Detector<Doorway> doorwayStomper;

        PlayerInputHandler playerInput;

        PathFinder finder;
        SquareMapper mapper;

        [SerializeField] Vector2Int defaultPathMax = new Vector2Int(6, 3);

        Utility.Logger logger;

        public void Start()
        {
            logger = new Utility.Logger("CastingLog", "CASTTESTOR");

            logger.LogLine("\n\n\n");
            logger.Log("LOG START!!");

            finder = new PathFinder();
            mapper = SquareMapper.instance;
            doorwayStomper = new Detector<Doorway>(transform, Vector3.down, LayerMask.GetMask("Block"));
            playerInput = transform.GetComponent<PlayerInputHandler>();
            playerInput.AddObserver(this);
        }

        public bool OnNotice()
        {
            Cast();

            return true;
        }

        void Cast()
        {
            var coord = MathOfWorld.PositionToCoordinate(transform.position);
            Square sqr;
            mapper.CheckCoordinate(coord, out sqr);
            logger.Log("CHECKED COORD");
            if (sqr == null)
            {
                TextPort.Display(0, "不明");
            }
            else
            {
                TextPort.Display(0, sqr.address.name);
                logger.Log("SQR COORD: " + sqr.coordinate + "MATH COORD: " + coord);
            }

            doorwayStomper.Scan(3, out var nowDoor);
            Vector3Int? direction;

            if (nowDoor != null && (direction = nowDoor.CheckAvalableDirection()) != null)
            {
                logger.Log("FINDING DOOR...");
                if (sqr == null)
                {
                    logger.LogError("!!ERROR!! SQUARE IS NULL", true);
                    return;
                }
                else
                {
                    logger.LogLine("SQUARE IS NOT NULL");
                }
                var targetDoor = finder.FindDoorway(sqr, direction.Value, defaultPathMax.x, defaultPathMax.y);




                if (targetDoor != null)
                {
                    logger.LogLine("FOUND!!");
                    logger.LogLine("FOUND DOOR INFO");
                    logger.LogLine("FROM::");
                    logger.LogLine(string.Format("      SQUARE NAME:{0} COORD:{1}", sqr.name, sqr.coordinate));
                    logger.LogLine(string.Format("     DOORWAY POS:{0}  DIRECTION:{1}", nowDoor.transform.position, nowDoor.avalableDirection));
                    logger.LogLine(string.Format("TO::"));
                    logger.LogLine(string.Format("     DOORWAY POS:{0}  DIRECTION:{1}", targetDoor.transform.position, targetDoor.avalableDirection));

                    var rawPath = targetDoor.transform.position - nowDoor.transform.position;
                    var startDirection = nowDoor.avalableDirection;
                    var endDirection = targetDoor.avalableDirection * -1;
                    var data = RoadServer.instance.GetRoadsByPath(rawPath, startDirection.Value, endDirection.Value);

                    if (data == null)
                    {
                        logger.LogLine("FAILED TO MAKE ROAD");
                    }
                    else
                    {
                        logger.LogLine("SUCCESS! MAKING ROAD");
                        RoadBuilder.instance.BuildRoad(data, nowDoor.transform.position, nowDoor.avalableDirection.Value);
                    }

                }
                else
                {
                    logger.LogLine("NOT FOUND");
                }
            }
        }
    }
}