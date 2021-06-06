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

        //信号を読み取る間隔であり、1mを進む時間でもある
        [SerializeField] float unitTick;
        ModuleState state;


        //速度単位はm/s
        [SerializeField] float speed = 2;


        void Start()
        {
            easer = new Easer();
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
            var worldDirection = transform.rotation * direction;
            do
            {
                yield return null;
                transform.parent.position += worldDirection * speed * Time.deltaTime;
            }
            while (state == ModuleState.working);

            //以下スナッピング
            var targetSpot = transform.parent.position;

            //向きベースで充填
            targetSpot.x += Mathf.Sign(direction.x);
            targetSpot.y += Mathf.Sign(direction.y);

            //切り捨て
            targetSpot = Vector3Int.FloorToInt(targetSpot);
            var delta = targetSpot - transform.parent.position;

            while (Vector3.Distance(transform.parent.position, targetSpot) > Vector3.kEpsilon)
            {
                Vector3.MoveTowards(transform.parent.position, targetSpot, speed * Time.deltaTime);
                yield return null;
            }
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