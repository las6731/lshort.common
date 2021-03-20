using System;
using MongoDB.Bson.Serialization.Attributes;

namespace LShort.Common.Models
{
    public abstract class ModelBase
    {
        /// <summary>
        /// The id.
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }

        public ModelBase()
        {
            Id = Guid.NewGuid();
        }
    }
}