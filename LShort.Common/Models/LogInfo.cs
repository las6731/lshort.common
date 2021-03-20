using System;
using System.Collections.Generic;
using System.Dynamic;
using MongoDB.Bson.Serialization.Attributes;

namespace LShort.Common.Models
{
    public class LogInfo : ModelBase
    {
        [BsonElement("level")]
        public string Level;

        [BsonElement("source")]
        [BsonIgnoreIfNull]
        public string Source;

        [BsonElement("message")]
        public string Message;

        [BsonExtraElements]
        public IList<KeyValuePair<string, object>> Properties;

        [BsonElement("details")]
        [BsonIgnoreIfNull]
        public object Details;

        [BsonElement("exception")]
        [BsonIgnoreIfNull]
        public Exception Exception;

        [BsonElement("utc")]
        public DateTime Utc;

        public LogInfo()
        {
            Utc = DateTime.UtcNow;
            Properties = new List<KeyValuePair<string, object>>();
        }
        
        public LogInfo(LogInfo baseInfo)
        {
            this.Level = baseInfo.Level;
            this.Source = baseInfo.Source;
            this.Properties = new List<KeyValuePair<string, object>>();
            foreach (var (key, value) in baseInfo.Properties)
            {
                Properties.Add(new KeyValuePair<string, object>(key, value));
            }
            this.Utc = DateTime.UtcNow;
        }

        public ExpandoObject ToDynamic()
        {
            var logItem = new ExpandoObject();
            var log = logItem as IDictionary<string, object>;
            
            log.Add("level", Level);
            log.Add("utc", Utc);
            
            if (Source != null)
            {
                log.Add("source", Source);
            }
            
            log.Add("message", Message);
            
            foreach (var prop in Properties)
            {
                log.Add(prop.Key, prop.Value);
            }

            if (Details != null)
            {
                log.Add("details", Details);
            }

            if (Exception != null)
            {
                log.Add("exception", Exception);
            }

            return logItem;
        }
    }
}