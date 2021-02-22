using System.Collections.Generic;
using System.Threading.Tasks;

namespace JH.Twitter
{
    /// <summary>
    /// Created an inmemory Queue for storing the twitter stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueStorageService<T> : IQueueStorageService<T>
    {
        private static Queue<T> messages = new Queue<T>();

        /// <summary>
        /// Add data to the queue that need to be processed
        /// </summary>
        /// <param name="input">Data to be queued</param>
        public void Enqueue(T input)
        {
            messages.Enqueue(input);
        }
        
        /// <summary>
        /// Retrieve the data from the queue for processing.
        /// </summary>
        /// <returns>Will return the next available item in the queue.</returns>
        public T Dequeue()
        {
            if(messages.Count > 0)
                return messages.Dequeue();
            return default(T);
        }
    }
}
