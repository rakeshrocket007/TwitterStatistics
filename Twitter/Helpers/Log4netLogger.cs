using JH.Twitter.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JH.Twitter.Helpers
{
    public class Log4netLogger : ILogger
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }
    }
}
