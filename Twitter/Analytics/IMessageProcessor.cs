namespace JH.Twitter.Analytics
{
    public interface IMessageProcessor
    {
        void Process(TwitterStreamResponse input);
    }
}