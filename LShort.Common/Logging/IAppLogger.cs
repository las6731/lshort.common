using System;

namespace LShort.Common.Logging
{
    public interface IAppLogger
    {
        void Information(string message);

        void Information(string message, object details);

        void Warning(string message);

        void Warning(string message, object details);

        void Error(string message);

        void Error(string message, Exception e);

        void Error(string message, object details);

        IAppLogger FromSource(Type source);

        IAppLogger WithCorrelation(Guid correlationId);

        IAppLogger WithProperty(string property, object value);
    }
}