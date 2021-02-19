using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JH.Twitter
{
    /// <summary>
    /// Statistics store that contain all the statistics. Ideally, it has to be another repository. 
    /// </summary>
    public class TwitterStatisticsModel : ICloneable
    {
        public int TweetCount { get; set; }

        public int TweetCountWithEmojis { get; set; }

        public int TweetCountWithUrl { get; set; }

        public int TweetCountWithPhoto { get; set; }
                
        public Dictionary<string, int> EmojiUsage { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> HashTagUsage { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> DomainUsage { get; set; } = new Dictionary<string, int>();

        public DateTime StorageInitatedTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// Retrieve the clone of the object so that we dont get dirty read data as the object is being used as a singleton.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var serializedData = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TwitterStatisticsModel>(serializedData);
        }
    }
}
