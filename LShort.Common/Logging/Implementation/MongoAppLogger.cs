using System;
using System.Collections.Generic;
using System.Dynamic;
using LShort.Common.Models;
using MongoDB.Driver;

namespace LShort.Common.Logging.Implementation
{
    public class MongoAppLogger : IAppLogger
    {
        private LogInfo LogBase;
        private IMongoDatabase db;
        private IMongoCollection<ExpandoObject> Collection => db.GetCollection<ExpandoObject>("Logs");

        public MongoAppLogger(IMongoDatabase db)
        {
            this.db = db;
            LogBase = new LogInfo();
        }

        public MongoAppLogger(IMongoDatabase db, LogInfo logBase)
        {
            this.db = db;
            LogBase = logBase;
        }
        
        private async void Insert(LogInfo logInfo)
        {
            var logItem = logInfo.ToDynamic();
            
            try
            {
                await Collection.InsertOneAsync(logItem);
            }
            catch (MongoWriteConcernException e)
            {
                Console.Error.WriteLine($"Failed to write log item to db: {logInfo}");
                Console.Error.WriteLine($"Log Write Exception: {e}");
            }
        }

        public void Information(string message)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Information",
                Message = message
            };
            Insert(logInfo);
        }

        public void Information(string message, object details)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Information",
                Message = message,
                Details = details
            };
            Insert(logInfo);
        }

        public void Warning(string message)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Warning",
                Message = message
            };
            Insert(logInfo);
        }

        public void Warning(string message, object details)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Warning",
                Message = message,
                Details = details
            };
            Insert(logInfo);
        }

        public void Error(string message)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Error",
                Message = message
            };
            Insert(logInfo);
        }

        public void Error(string message, Exception e)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Error",
                Message = message,
                Exception = e
            };
            Insert(logInfo);
        }

        public void Error(string message, object details)
        {
            var logInfo = new LogInfo(LogBase)
            {
                Level = "Error",
                Message = message,
                Details = details
            };
            Insert(logInfo);
        }

        public IAppLogger FromSource(Type source)
        {
            return new MongoAppLogger(db, new LogInfo(LogBase)
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
            var logger = new MongoAppLogger(db, new LogInfo(LogBase));
            logger.LogBase.Properties.Add(new KeyValuePair<string, object>(property, value));
            return logger;
        }
    }
}