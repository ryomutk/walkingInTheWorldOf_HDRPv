using UnityEngine;
using System.Linq;
using World.Building;

namespace World.Path
{
    /// <summary>
    /// 建物の上にあるDoorway。
    /// プレイヤーが踏んだ時、空いているということがないようにする
    /// →つまり、ここへあがってくる階段は作れても下る階段は出せないようにする。
    /// かなりアドホック的な感じになるのであれ。
    /// </summary>
    public class HighDoorWay : Doorway
    {
        Structure parent { get; set; }
        public bool used { get; private set; }

        void Start()
        {
            parent = GetComponentInParent<Structure>();
        }



        public override Vector3Int? CheckAvalableDirection()
        {
            var direction = base.CheckAvalableDirection();
            if (direction != null)
            {
                //もし天井の高さがプレイヤーより低かったら
                if (Physics.Raycast(transform.position, Vector3.up, Player.size.y, ~LayerMask.GetMask("Player")))
                {
                    return null;
                }


                //もし誰か使われている人がいるなら
                if (parent.doorwayList.Any(x => x is HighDoorWay x1 && x1.used))
                {
                    return null;
                }


                //でなければ
                used = true;
                return direction;
            }

            return null;
        }
    }
}