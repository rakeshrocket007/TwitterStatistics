using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JH.Twitter
{
    /// <summary>
    /// Parent class denoting the data node of the Twitter stream
    /// </summary>
    public class TwitterStreamResponse
    {
        public TwitterStreamChunk data { get; set; }
    }

    /// <summary>
    /// Actual data regarding the tweets.
    /// </summary>
    public class TwitterStreamChunk
    {
        public string id { get; set; }

        public string text { get; set; }
    }
}
