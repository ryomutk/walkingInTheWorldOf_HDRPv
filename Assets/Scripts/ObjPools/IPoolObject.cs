

public interface IPoolObject<T>
{
    bool CreatePool(T obj,int num);
    T GetObj(bool activate = true);
}
