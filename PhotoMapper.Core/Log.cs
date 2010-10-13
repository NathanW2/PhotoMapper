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
        /// <summary>
        /// Creates an log4net <see cref="ILog"/> for the provided type.
        /// </summary>
        public static Func<Type, ILog> GetLog = type => log4net.LogManager.GetLogger(type.GetType());
    }
}
