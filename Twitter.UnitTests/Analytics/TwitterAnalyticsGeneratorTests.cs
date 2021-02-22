using JH.Twitter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Twitter.UnitTests.Analytics
{
    [TestClass]
    public class TwitterAnalyticsGeneratorTests
    {
        [TestMethod]
        public void GenerateAnalytics_Success()
        {
            var twitterStatistics = new TwitterStatisticsModel();
            twitterStatistics.TweetCount = 10;
            twitterStatistics.TweetCountWithEmojis = 4;
            twitterStatistics.TweetCountWithPhoto = 1;
            twitterStatistics.TweetCountWithUrl = 3;
            twitterStatistics.EmojiUsage = new Dictionary<string, int>();
            twitterStatistics.EmojiUsage.Add("Sample Smiley 1", 4);
            twitterStatistics.EmojiUsage.Add("Sample Smiley 2", 2);
            var analyticsGenerator = new TwitterAnalyticsGenerator(twitterStatistics);
            var result = analyticsGenerator.GenerateAnalytics();

            Assert.AreEqual(result["Total tweets with emojis"].ToString(), twitterStatistics.TweetCountWithEmojis.ToString());
            Assert.AreEqual(result["Tweet Count"].ToString(), twitterStatistics.TweetCount.ToString());
            Assert.AreEqual(result["Top Emoji in tweets"].ToString(), "Sample Smiley 1");
        }
    }
}
