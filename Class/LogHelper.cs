using System;
using System.Collections.Generic;
using System.Text;

namespace Clipboards.Class
{

    internal class LogHelper
    {
        public readonly NLog.Logger _logger;
        public static readonly LogHelper Instance = new LogHelper();
        public LogHelper()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }
    }
}
