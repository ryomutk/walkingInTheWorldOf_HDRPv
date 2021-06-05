using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Math;

namespace World.Rader.Core
{
    public class Detector<T>
    {
        float baseMagnitude
        {
            get
            {
                if (fullRange)
                {
                    return (MathOfWorld.gridSize.y - 1);
                }
                return MathOfWorld.gridSize.y / 2;
            }
        }

        Vector3 castVector
        {
            get
            {
                return _direction * baseMagnitude;
            }
        }
        Vector3 _direction;
        Transform transform;
        int layerMask;
        bool fullRange = false;

        /// <summary>
        /// </summary>
        /// <param name="transform">発信者のtransform</param>
        /// <param name="layerMask">検知したいレイヤー入力しなければすべて</param>
        /// <param name="fullRange">full = square1個分のレンジ。false ならhalfRange</param>
        public Detector(Transform transform, Vector3 direction, int layerMask = 0, bool fullRange = false)
        {
            this.transform = transform;
            this.layerMask = layerMask;
            this.fullRange = fullRange;
            _direction = direction;
            _direction.Normalize();
        }

        /// <summary>
        /// 自分の上へRayを都バス
        /// </summary>
        /// <returns></returns>
        public T Stomp()
        {
            RaycastHit hit;

            if (layerMask == 0)
            {
                Physics.Raycast(transform.position, castVector, out hit, castVector.magnitude);
            }
            else
            {
                Physics.Raycast(transform.position, castVector, out hit, castVector.magnitude, layerMask);
            }

            Debug.DrawRay(transform.position, castVector, Color.red, 2);

            if (hit.transform == null)
            {
                return default(T);
            }

            var comp = hit.transform.GetComponent<T>();

            return comp;
        }

        /// <summary>
        /// boxcastを使う 
        /// </summary>
        /// <param name="size">size四方のboxをcast</param>
        /// <returns></returns>
        public bool Scan(float size)
        {
            Vector3 boxSize = Vector3.one;
            boxSize.y = 0;
            boxSize *= size;

            if (layerMask == 0)
            {
                return Physics.BoxCast(transform.position, boxSize / 2, castVector, Quaternion.identity, baseMagnitude);
            }
            else
            {
                return Physics.BoxCast(transform.position, boxSize / 2, castVector, Quaternion.identity, baseMagnitude, layerMask);
            }

        }

        public bool Scan(float size, out T output)
        {
            RaycastHit hit;
            Vector3 boxSize = Vector3.one;
            bool result;
            boxSize.y = 0;
            boxSize *= size;

            if (layerMask == 0)
            {
                result = Physics.BoxCast(transform.position, boxSize / 2, castVector, out hit, Quaternion.identity, baseMagnitude);
            }
            else
            {
                result = Physics.BoxCast(transform.position, boxSize / 2, castVector, out hit, Quaternion.identity, baseMagnitude, layerMask);
            }

            if (result)
            {
                output = hit.transform.GetComponent<T>();
                return true;
            }

            output = default(T);
            return false;
        }
    }

}