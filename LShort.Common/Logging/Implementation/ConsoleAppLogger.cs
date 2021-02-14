using System;
using System.Collections.Generic;
using System.Text;
using LShort.Common.Models;

namespace LShort.Common.Logging.Implementation
{
    public class ConsoleAppLogger : IAppLogger
    {
        private LogInfo LogBase;

        public ConsoleAppLogger()
        {
            LogBase = new LogInfo();
        }

        public ConsoleAppLogger(LogInfo logBase)
        {
            LogBase = logBase;
        }
        
        public void Information(string message)
        {
            var sb = new StringBuilder();
            if (LogBase.Source != null)
            {
                sb.Append($"{LogBase.Source}: ");
            }

            sb.Append(message);
            Console.WriteLine(sb.ToString());
        }

        public void Information(string message, object details)
        {
            var sb = new StringBuilder();
            if (LogBase.Source != null)
            {
                sb.Append($"{LogBase.Source}: ");
            }

            sb.Append(message);
            sb.Append($" : {details}");
            Console.WriteLine(sb.ToString());
        }

        public void Warning(string message)
        {
            Information(message);
        }

        public void Warning(string message, object details)
        {
            Information(message, details);
        }

        public void Error(string message)
        {
            var sb = new StringBuilder();
            if (LogBase.Source != null)
            {
                sb.Append($"{LogBase.Source}: ");
            }

            sb.Append(message);
            Console.Error.WriteLine(sb.ToString());
        }

        public void Error(string message, Exception e)
        {
            Error(message, (object) e);
        }

        public void Error(string message, object details)
        {
            var sb = new StringBuilder();
            if (LogBase.Source != null)
            {
                sb.Append($"{LogBase.Source}: ");
            }

            sb.Append(message);
            sb.Append($" : {details}");
            Console.Error.WriteLine(sb.ToString());
        }

        public IAppLogger FromSource(Type source)
        {
            return new ConsoleAppLogger(new LogInfo(LogBase)
            {
                Source = source.Name
            });
        }

        public IAppLogger WithCorrelation(Guid correlationId)
        {
            return WithProperty("correlationId", correlationId);
        }

        public IAppLogger WithProperty(string property, object value)
        {
            var logger = new ConsoleAppLogger(new LogInfo(LogBase));
            logger.LogBase.Properties.Add(new KeyValuePair<string, object>(property, value));
            return logger;
        }
    }
}