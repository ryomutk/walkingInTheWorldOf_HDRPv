
namespace ObserverPattern
{
    public interface IMediator<T>
    {
        /// <summary>
        /// 昔懐かし、そのままdataを投げるタイプ
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="M"></typeparam>
        void SimpleNotice<M>(M data);

        /// <summary>
        /// 無型の信号を発信
        /// </summary>
        /// <param name="source"></param>
        void OnNotice(T source);

        /// <summary>
        /// M型の信号を発信。
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="M"></typeparam>
        void OnNotice<M>(T source);


        /// <summary>
        /// 全人類のNoticeAll
        /// </summary>
        /// <param name="source"></param>
        void NoticeAll(T source);

        bool AddObserver(IObserverBase observerBase);
        bool RemoveObserver(IObserverBase observerBase);
    }
}