public interface IObserverBase{}
public interface IObserver<T>:IObserverBase
{
    bool OnNotice(T arg);
}

public interface IObserver:IObserverBase
{
    bool OnNotice();
}