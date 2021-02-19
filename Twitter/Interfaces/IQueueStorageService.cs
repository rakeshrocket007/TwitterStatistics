namespace JH.Twitter
{
    public interface IQueueStorageService<T>
    {
        T Dequeue();
        void Enqueue(T input);
    }
}