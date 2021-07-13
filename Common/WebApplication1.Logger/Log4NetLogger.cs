using System;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        public Log4NetLogger(string category, XmlElement configuration)
        {
            var loggerRepo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(Hierarchy));
            _log = LogManager.GetLogger(loggerRepo.Name, category);

            log4net.Config.XmlConfigurator.Configure(loggerRepo, configuration);
        }

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));
            if (!IsEnabled(logLevel)) 
                return;
            var logMessage = formatter(state, exception);
            if (string.IsNullOrEmpty(logMessage) && exception is null) 
                return;
            switch (logLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(logMessage);
                    break;
                case LogLevel.Information:
                    _log.Info(logMessage);
                    break;
                case LogLevel.Warning:
                    _log.Warn(logMessage);
                    break;
                case LogLevel.Error:
                    _log.Error(logMessage);
                    break;
                case LogLevel.Critical:
                    _log.Error(logMessage);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.None => false,
                LogLevel.Trace => _log.IsDebugEnabled,
                LogLevel.Debug => _log.IsDebugEnabled,
                LogLevel.Information => _log.IsInfoEnabled,
                LogLevel.Warning => _log.IsWarnEnabled,
                LogLevel.Error => _log.IsErrorEnabled,
                LogLevel.Critical => _log.IsFatalEnabled,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            };
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
