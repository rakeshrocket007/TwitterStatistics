using JH.Twitter.Analytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JH.Twitter
{
    /// <summary>
    /// Tweet Manager class is mainly to run background tasks which will initiate a process to fetch data from Twitter and Queue them. Another task to process the data. 
    /// It has to be instantiated as a singleton.
    /// </summary>
    /// <typeparam name="T">T is type of TwitterStreamResponse</typeparam>
    public class TweetManager<T> : ITweetManager<T> where T : TwitterStreamResponse
    {
        private ITwitterRepository _repository;
        private IQueueStorageService<T> _queueStorageService;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Consturctor for the class which takes in ITwitterRepository and QueueStorageService
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="queueStorageService"></param>
        public TweetManager(ITwitterRepository repository, IQueueStorageService<T> queueStorageService)
        {
            _repository = repository;
            _queueStorageService = queueStorageService;
        }

        /// <summary>
        /// Fetch data from Twitter and write it to the Store.
        /// </summary>
        /// <param name="endpoint">endpoint from where the data has to be read</param>
        /// <returns></returns>
        public async Task InitiateQueueService(string endpoint)
        {
            try
            {
                var streamReader = _repository.GetTwitterStream(endpoint);

                while (!streamReader.EndOfStream)
                {
                    _queueStorageService.Enqueue(JsonConvert.DeserializeObject<T>(streamReader.ReadLine()));
                }
            }
            catch(Exception ex)
            {
                _log.Error("Error occured during Fetching twitter data and writing to the Queue. " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Process data from the Queue.
        /// </summary>
        /// <param name="messageProcessor">Inject fuctionality as to what has to be processed from the retrieved data.</param>
        public void ProcessQueueService(IMessageProcessor messageProcessor)
        {
            while (true)
            {
                var message = _queueStorageService.Dequeue();
                if (message != null)
                    messageProcessor.Process(message);
            }

        }
    }
}
