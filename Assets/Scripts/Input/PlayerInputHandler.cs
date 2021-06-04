using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputHandler : SubjectBehaviour
{
    [SerializeField] static KeyCode forwardKey = KeyCode.W;
    [SerializeField] static KeyCode floatKey = KeyCode.Space;
    [SerializeField] static KeyCode sinkKey = KeyCode.LeftShift;
    [SerializeField] static KeyCode rightKey = KeyCode.D;
    [SerializeField] static KeyCode leftKey = KeyCode.A;
    [SerializeField] static KeyCode backKey = KeyCode.S;

    /// <summary>
    /// いちます動くごとにcast
    /// </summary>
    Vector3 lastCast;


    Dictionary<KeyCode, Vector3Int> keyPairs = new Dictionary<KeyCode, Vector3Int>()
    {
        {forwardKey,MathOfWorld.directions2D[3]},
        {backKey,MathOfWorld.directions2D[0]},
        {rightKey,Vector3Int.right},
        {leftKey,Vector3Int.left},
        {floatKey,Vector3Int.up},
        {sinkKey,Vector3Int.down}
    };


    Vector3 forward = new Vector3(0, 0, 1);
    ModuleState state;
    UFOMoveMotion motion;
    Rigidbody body;
    bool active = false;
    [SerializeField] bool activateOnStart = true;

    void Start()
    {
        motion = GetComponentInChildren<UFOMoveMotion>();
        body = GetComponent<Rigidbody>();
        if(activateOnStart)
        {
            Activate();
        }
    }

    public bool Activate()
    {
        if (!active)
        {
            active = true;
            foreach (var key in keyPairs.Keys)
            {
                if (key == forwardKey)
                {
                    StartCoroutine(ForwardCoroutine());
                }
                else
                {
                    StartCoroutine(SlideCoroutine(key));
                }
            }
            return true;
        }

        return false;
    }

    public bool Stop()
    {
        if (active)
        {
            active = false;
            return true;
        }

        return false;
    }

    void Forward()
    {
        if (Input.GetKey(forwardKey))
        {
            state = ModuleState.working;
            var direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * forward;

            motion.Move(direction);
            TextPort.Display(1, direction + "");
        }
        else if (state == ModuleState.working)
        {
            state = ModuleState.compleate;
            motion.Stop();
        }
    }

    IEnumerator ForwardCoroutine()
    {
        while (active)
        {
            Forward();
            yield return new WaitUntil(() => Clock.instance.Signal);
        }
    }

    IEnumerator SlideCoroutine(KeyCode key)
    {
        if (key == forwardKey)
        {
            yield break;
        }

        Tween tw;

        while (active)
        {
            if (Input.GetKey(key))
            {
                var direction = transform.rotation*keyPairs[key];
                tw = motion.Slide(direction);
                yield return tw.WaitForCompletion();
            }
            else
            {
                yield return new WaitUntil(() => Clock.instance.Signal);
            }
        }
    }

    void LateUpdate()
    {
        var delta = lastCast - transform.position;
        if (state == ModuleState.working && delta.sqrMagnitude > 1)
        {
            lastCast = transform.position;
            Notice();
        }
    }
}