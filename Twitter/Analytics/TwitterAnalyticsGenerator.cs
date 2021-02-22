using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JH.Twitter
{
    /// <summary>
    /// Generate analytics to the from the Statistics Store.
    /// </summary>
    public class TwitterAnalyticsGenerator : IAnalyticsGenerator
    {
        private TwitterStatisticsModel _statistics;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for the Analytics Generator. Input is the Statistics. 
        /// </summary>
        /// <param name="statistics"></param>
        public TwitterAnalyticsGenerator(TwitterStatisticsModel statistics)
        {            
            _statistics = (TwitterStatisticsModel)statistics.Clone();
        }

        /// <summary>
        /// Functionality to generate metrics from the statistics store.
        /// </summary>
        /// <returns>Dictionary containing the metric name as key and it's corresponding value</returns>
        public Dictionary<string, string> GenerateAnalytics()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                result.Add("Tweet Count", _statistics.TweetCount.ToString());

                int avg = _statistics.TweetCount / ((DateTime.Now - _statistics.StorageInitatedTime).Hours + 1);
                result.Add("Hourly average of tweets", avg.ToString());

                result.Add("Top Emoji in tweets", _statistics.EmojiUsage.Count > 0 ? _statistics.EmojiUsage.Aggregate((l, r) => l.Value > r.Value ? l : r).Key : string.Empty);
                result.Add("Top Hashtag in tweets", _statistics.HashTagUsage.Count > 0 ? _statistics.HashTagUsage.Aggregate((l, r) => l.Value > r.Value ? l : r).Key : string.Empty);
                result.Add("Top Domain in tweets", _statistics.DomainUsage.Count > 0 ? _statistics.DomainUsage.Aggregate((l, r) => l.Value > r.Value ? l : r).Key : string.Empty);

                float emojiPercentage = (_statistics.TweetCountWithEmojis * 100.0f) / _statistics.TweetCount;
                result.Add("Total tweets with emojis", _statistics.TweetCountWithEmojis.ToString());
                result.Add("Percentage of tweets with emojis", Math.Round(emojiPercentage, 2).ToString());

                float urlPercentage = (_statistics.TweetCountWithUrl * 100.0f) / _statistics.TweetCount;
                result.Add("Percentage of tweets with Url", Math.Round(urlPercentage, 2).ToString());

                float picturePercentage = (_statistics.TweetCountWithPhoto * 100.0f) / _statistics.TweetCount;
                result.Add("Percentage of tweets with pictures", Math.Round(picturePercentage, 2).ToString());
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                _log.Info("Error occured while generating Analytics : " + JsonConvert.SerializeObject(_statistics));
                throw;
            }

            return result;
        }


    }
}
