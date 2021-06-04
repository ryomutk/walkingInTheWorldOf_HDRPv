using UnityEngine;

//普通のobjPool
//InstantPool
public abstract class ObjPool<T> : Singleton<ObjPool<T>>, IPoolObject<T>
where T : MonoBehaviour
{
    InstantPool<T> pool;
    protected abstract Transform poolTransform{get;}

    protected override void Awake()
    {
        base.Awake();
        pool = new InstantPool<T>(poolTransform);
    }

    public bool CreatePool(T obj, int num)
    {
        return pool.CreatePool(obj, num);
    }

    public T GetObj(bool activate = true)
    {
        return pool.GetObj(activate);
    }
}
