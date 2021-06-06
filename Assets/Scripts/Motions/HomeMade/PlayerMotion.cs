using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using ModulePattern;
using System.Collections;

namespace Motion
{
    public class PlayerMotion : MonoBehaviour, IMoveMotion
    {
        Easer easer;
        [SerializeField] DG.Tweening.Ease moveStartEase;
        [SerializeField] DG.Tweening.Ease moveEndEase;
        ModuleState state;


        //速度単位はm/s
        [SerializeField] float speed = 2;
        Utility.Logger logger;


        void Start()
        {
            easer = new Easer();
            logger = new Utility.Logger("MoveLog","PLAYER MOTION");
        }

        public bool Move(Vector3 direction)
        {
            if (state != ModuleState.working)
            {
                direction.Normalize();
                state = ModuleState.working;
                StartCoroutine(MoveCoroutine(direction));
                return true;
            }

            return false;
        }

        //Stopが呼ばれるまで動き続ける
        IEnumerator MoveCoroutine(Vector3 direction)
        {
            logger.Log("MOVE START");
            do
            {
                yield return null;
                transform.parent.position += transform.rotation * direction * speed * Time.deltaTime;
            }
            while (state == ModuleState.working);

            //以下スナッピング
            var targetSpot = transform.parent.position;

            //切り捨て
            targetSpot = Vector3Int.RoundToInt(targetSpot);
            logger.LogLine("snap target is:"+targetSpot);

            while (Vector3.Distance(transform.parent.position, targetSpot) > Vector3.kEpsilon)
            {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetSpot, speed * Time.deltaTime);
                yield return null;
            }
            logger.LogLine("MOVE END");
        }

        public bool Slide(Vector3 direction)
        {
            return false;
        }

        public bool Stop()
        {
            if (state == ModuleState.working)
            {
                state = ModuleState.compleate;
                return true;
            }
            return false;
        }

    }
}