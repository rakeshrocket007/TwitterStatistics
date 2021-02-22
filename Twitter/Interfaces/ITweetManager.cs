using JH.Twitter.Analytics;
using System.Threading.Tasks;

namespace JH.Twitter
{
    public interface ITweetManager<T>
    {
        void InitiateQueueService(string endpoint);

        void ProcessQueueService(IMessageProcessor messageProcessor);
    }
}