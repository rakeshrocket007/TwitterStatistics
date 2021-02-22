using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JH.Twitter
{
    /// <summary>
    /// Perform data operations on Twitter
    /// </summary>
    public class TwitterRepository : ITwitterRepository
    {
        private TwitterMetadata _metadata;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TwitterRepository(TwitterMetadata metadata)
        {
            _metadata = metadata;
        }

        /// <summary>
        /// Fetch data from twitter endpoint.
        /// </summary>
        /// <param name="endpoint">Endpoint of twitter api</param>
        /// <returns>StreamReader pointing to the twitter stream</returns>
        public StreamReader GetTwitterStream(string endpoint)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _metadata.BearerToken);
                    var stream = httpClient.GetStreamAsync(endpoint).Result;

                    var reader = new StreamReader(stream, Encoding.UTF8);

                    return reader;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error occured during initiating the call to the Twitter service." + ex.InnerException.Message);
                throw;
            }
        }
    }
        
}
