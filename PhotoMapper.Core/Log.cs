using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace PhotoMapper.Core
{
    public delegate TResult Func<T, TResult>(T t);

    /// <summary>
    /// Used to manage logging.
    /// </summary>
    public static class Logging
    {
        public static Func<String, ILog> GetLog = name => log4net.LogManager.GetLogger(name);
        public static ILog log = log4net.LogManager.GetLogger("PhotoMapper");
    }
}
