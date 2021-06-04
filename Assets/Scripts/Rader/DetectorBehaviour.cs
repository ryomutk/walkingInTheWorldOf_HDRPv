using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T">検知するもの</typeparam>
public class DetectorBehaviour<T> : SubjectBehaviour<ObjectDataSet<T>>
{
    Detector<T> rader;

    [SerializeField] Vector3Int direction = Vector3Int.up;
    [SerializeField] CastType _castType = CastType.box;
    [SerializeField] float boxSize = 3;
    [SerializeField] string maskName = null;

    public CastType castType{get{return _castType;}}
    System.Func<T> castMethod = null;


    protected virtual void Start()
    {
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
            ObjectDataSet<T> dataSet = new ObjectDataSet<T>(gameObject, result);
            Notice(dataSet);
            return true;
        }

        return false;
    }

    public bool Cast(out T hit)
    {
        hit = castMethod();

        if (hit != null)
        {
            ObjectDataSet<T> dataSet = new ObjectDataSet<T>(gameObject, hit);
            Notice(dataSet);
            return true;
        }

        hit = default(T);
        return false;
    }

}

/// <summary>
/// ObserServerで引数に自分とデータを投げたーい
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectDataSet<T>
{
    GameObject gameObject { get; set; }
    T data { get; set; }

    public ObjectDataSet(GameObject gameObject, T data)
    {
        this.gameObject = gameObject;
        this.data = data;
    }
}

public enum CastType
{
    ray,
    box
}