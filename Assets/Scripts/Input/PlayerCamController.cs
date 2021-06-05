using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{

    [RequireComponent(typeof(Camera))]
    public class PlayerCamController : MonoBehaviour
    {
        new Camera camera;
        // Start is called before the first frame update
        [SerializeField] float multiplier = 3;
        [SerializeField] Vector2 maxAngles = new Vector2(80, 80);
        void Start()
        {
            camera = GetComponent<Camera>();
        }

        void Update()
        {
            var nowEuler = camera.transform.parent.eulerAngles;
            if (nowEuler.x > 180)
            {
                nowEuler.x = nowEuler.x - 360;
            }
            if (nowEuler.y > 180)
            {
                nowEuler.y = nowEuler.y - 360;
            }
            nowEuler.y += MouceDetector.instance.delta.x * multiplier;
            nowEuler.x -= MouceDetector.instance.delta.y * multiplier;

            if (maxAngles.x != -1)
            {
                if (nowEuler.x > maxAngles.x)
                {
                    nowEuler.x = maxAngles.x;
                }
                else if (nowEuler.x < -maxAngles.x)
                {
                    nowEuler.x = -maxAngles.x;
                }
            }

            if (maxAngles.y != -1)
            {
                if (nowEuler.y > maxAngles.y)
                {
                    nowEuler.y = maxAngles.y;
                }
                else if (nowEuler.y < -maxAngles.y)
                {
                    nowEuler.y = -maxAngles.y;
                }
            }


            camera.transform.parent.eulerAngles = nowEuler;
        }
    }

}