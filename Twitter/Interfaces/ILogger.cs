using System;
using System.Collections.Generic;
using System.Text;

namespace JH.Twitter.Interfaces
{
    public interface ILogger
    {
        void Error(string message);

        void Info(string message);
    }
}
