    namespace ObserverPattern
{
    public interface ISubject<T>
    {
        bool AddObserver(IObserver<T> observer);
        bool RemoveObserver(IObserver<T> observer);
    }

    public interface ISubject
    {
        bool AddObserver(IObserver observer);
        bool RemoveObserver(IObserver observer);
    }
}