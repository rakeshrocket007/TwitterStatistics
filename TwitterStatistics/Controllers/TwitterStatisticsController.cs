using JH.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TwitterStatistics.Controllers
{
    [RoutePrefix("api/twitter")]
    public class TwitterStatisticsController : ApiController
    {
        private IAnalyticsGenerator _analyticsRepository;

        public TwitterStatisticsController(IAnalyticsGenerator analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }
        
        /// <summary>
        /// Fetches Analytical data
        /// </summary>
        /// <returns>Analytical data on successful execution with status code 200</returns>
        [Route("statistics")]
        public HttpResponseMessage GetStatistics()
        {
            var result = new Dictionary<string, string>();
            try
            {
                result = _analyticsRepository.GenerateAnalytics();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error occured while fetching the analytics data");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
