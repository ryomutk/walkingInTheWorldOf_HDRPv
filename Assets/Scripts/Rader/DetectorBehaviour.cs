using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T">検知するもの</typeparam>
public class DetectorBehaviour<T> : MonoBehaviour
{
    Detector<T> rader;

    [SerializeField] Vector3Int direction = Vector3Int.up;
    [SerializeField] CastType _castType = CastType.box;
    [SerializeField] float boxSize = 3;
    [SerializeField] string maskName = null;

    public CastType castType { get { return _castType; } }
    System.Func<T> castMethod = null;

    public IMediator<DetectorBehaviour<T>> mediator { get { return _mediator;} }

    IMediator<DetectorBehaviour<T>> _mediator;


    protected virtual void Start()
    {
        _mediator = new DetectorMediator<T>();

        if (maskName == null)
        {
            rader = new Detector<T>(transform, direction, fullRange: false);
        }
        else
        {
            rader = new Detector<T>(transform, direction, LayerMask.GetMask(maskName), false);
        }

    }

    public void SetCastType(CastType castType)
    {
        this._castType = castType;
        if (castType == CastType.ray)
        {
            castMethod = () => rader.Stomp();
        }
        else if (castType == CastType.box)
        {
            castMethod = () =>
            {
                rader.Scan(boxSize, out T T);
                return T;
            };
        }
    }


    public bool Cast()
    {
        T result = default(T);

        result = castMethod();

        if (result != null)
        {
            _mediator.SimpleNotice(result);
            _mediator.NoticeAll(this);
            return true;
        }

        return false;
    }

    public bool Cast(out T hit)
    {
        hit = default(T);

        hit = castMethod();

        if (hit != null)
        {
            _mediator.SimpleNotice(hit);
            _mediator.NoticeAll(this);
            return true;
        }


        return false;
    }

}

public enum CastType
{
    ray,
    box
}