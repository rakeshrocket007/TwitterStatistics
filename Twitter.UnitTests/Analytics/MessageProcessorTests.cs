using JH.Twitter;
using JH.Twitter.Analytics;
using JH.Twitter.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Twitter.UnitTests.Analytics
{
    [TestClass]
    public class MessageProcessorTests
    {
        [TestMethod]
        public void ProcessMethodWithUrl_Succes()
        {
            var sampleTweet = new TwitterStreamResponse();
            sampleTweet.data = new TwitterStreamChunk();
            sampleTweet.data.id = "1";
            sampleTweet.data.text = "test www.google.com is the website";
            var mockLogger = new Mock<ILogger>();
            var emojiStore = new EmojiStore(mockLogger.Object, @"./Data/emoji.json");

            var messageProcessor = new MessageProcessor(emojiStore, mockLogger.Object, new TwitterStatisticsModel());
            messageProcessor.Process(sampleTweet);

            Assert.AreEqual(messageProcessor._statistics.TweetCountWithUrl, 1);
        }

        [TestMethod]
        public void ProcessMethodWithEmoji_Success()
        {
            var sampleTweet = new TwitterStreamResponse();
            sampleTweet.data = new TwitterStreamChunk();
            sampleTweet.data.id = "1";
            sampleTweet.data.text = "@citrix 👍";
            var mockLogger = new Mock<ILogger>();
            var emojiStore = new EmojiStore(mockLogger.Object, @"./Data/emoji.json");

            var messageProcessor = new MessageProcessor(emojiStore, mockLogger.Object, new TwitterStatisticsModel());
            messageProcessor.Process(sampleTweet);

            Assert.AreEqual(messageProcessor._statistics.TweetCountWithEmojis, 1);
        }
    }
}
