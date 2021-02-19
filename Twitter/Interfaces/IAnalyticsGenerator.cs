using System;
using System.Collections.Generic;
using System.Text;

namespace JH.Twitter
{
    public interface IAnalyticsGenerator
    {
        Dictionary<string,string> GenerateAnalytics();
    }
}
