using JH.Twitter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JH.Twitter.Analytics
{
    /// <summary>
    /// Processes the tweets and creates the statistical data
    /// </summary>
    public class MessageProcessor : IMessageProcessor
    {
        public TwitterStatisticsModel _statistics { get; private set; }
        private ILogger _log;
        private IEmojiStore _emojiStore;

        /// <summary>
        /// Constructor for the class that takes in the Statistics store.
        /// </summary>
        /// <param name="emojiStore">Contains list of emojis</param>
        /// <param name="statistics">Statistics model</param>
        public MessageProcessor(IEmojiStore emojiStore, ILogger log, TwitterStatisticsModel statistics)
        {
            _statistics = statistics;
            _log = log;
            _emojiStore = emojiStore;
        }

        /// <summary>
        /// Method to process each message.
        /// </summary>
        /// <param name="input">Data that has to be processed</param>
        public void Process(TwitterStreamResponse input)
        {
            var statisticsCopy = (TwitterStatisticsModel)_statistics.Clone();
            try
            {
                // Tweet count
                _statistics.TweetCount++;

                // url and domain count
                var urlOccurences = FindOccurences_Regex(input.data.text, @"(http|http(s)?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?");
                _statistics.TweetCountWithUrl += urlOccurences.Count == 0 ? 0 : 1;
                foreach (var occurence in urlOccurences)
                {
                    if (_statistics.DomainUsage.ContainsKey(occurence))
                        _statistics.DomainUsage[occurence] += 1;
                    else
                        _statistics.DomainUsage.Add(occurence, 1);
                }

                // hashtags
                var hashtagOccurences = FindOccurences_Regex(input.data.text, @"(?<!\w)#\w+");
                foreach (var occurence in hashtagOccurences)
                {
                    if (_statistics.HashTagUsage.ContainsKey(occurence))
                        _statistics.HashTagUsage[occurence] += 1;
                    else
                        _statistics.HashTagUsage.Add(occurence, 1);
                }

                // photos
                _statistics.TweetCountWithPhoto += input.data.text.Contains("pic.twitter.com") ? 1 : 0;
                _statistics.TweetCountWithPhoto += input.data.text.Contains("pic.") ? 1 : 0;

                // TBD emoji count
                var emojiOccurence = FindEmojis(input.data.text);
                _statistics.TweetCountWithEmojis += emojiOccurence.Count == 0 ? 0 : 1;
                foreach (var occurence in emojiOccurence)
                {
                    if (_statistics.EmojiUsage.ContainsKey(occurence))
                        _statistics.EmojiUsage[occurence] += 1;
                    else
                        _statistics.EmojiUsage.Add(occurence, 1);
                }
            }
            catch (Exception ex)
            {
                _statistics = statisticsCopy;
                _log.Error("Error occured during processing a tweet." + ex.Message);
                _log.Info("Failed logging the tweet : " + input.data.ToString());
            }
        }               

        /// <summary>
        /// Functionality to check for regex and return the occurences
        /// </summary>
        /// <param name="text">text to be checked</param>
        /// <param name="expr">regex expression</param>
        /// <returns></returns>
        private List<string> FindOccurences_Regex(string text, string expr)
        {
            List<string> result = new List<string>();
            MatchCollection mc = Regex.Matches(text, expr);

            foreach (Match m in mc)
            {
                result.Add(m.Value);
            }
            return result;
        }

        /// <summary>
        /// Get list of emojis from the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> FindEmojis(string text)
        {
            var emojisInTweet = _emojiStore.items.Where(e => text.Contains(e.unifiedCh));
            if(emojisInTweet != null && emojisInTweet.Count() > 0)
            {
                return emojisInTweet.Select(e => e.name).ToList();
            }
            return new List<string>();
        }
    }
}
