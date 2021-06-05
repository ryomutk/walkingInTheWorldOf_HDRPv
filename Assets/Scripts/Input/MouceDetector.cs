using UnityEngine;
using Utility;

namespace InputSystem
{

    public class MouceDetector : Singleton<MouceDetector>
    {
        Vector2 lastPosition = new Vector3();
        Vector2 position = new Vector3();
        public Vector2 delta { get { return _delta; } }
        [SerializeField] float deltaPower = 1.5f;
        Vector2 _delta = new Vector3();
        void Update()
        {
            position = Input.mousePosition;
            _delta = position - lastPosition;

            _delta.x = MagnPow(_delta.x, deltaPower);
            _delta.y = MagnPow(_delta.y, deltaPower);
        }

        void LateUpdate()
        {
            lastPosition = position;
        }

        /// <summary>
        /// 符号を変えずに絶対値のみ乗算する
        /// </summary>
        /// <param name="f"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        float MagnPow(float f, float p)
        {
            var sign = 1;
            if (f < 0)
            {
                sign = -1;
                f *= -1;
            }

            f = Mathf.Pow(f, p);
            f *= sign;

            return f;
        }
    }
}