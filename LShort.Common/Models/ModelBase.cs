﻿using System;
using MongoDB.Bson.Serialization.Attributes;

namespace LShort.Common.Models
{
    public abstract class ModelBase
    {
        /// <summary>
        /// The id.
        /// </summary>
        [BsonId]
        public Guid Id;

        protected ModelBase()
        {
            Id = Guid.NewGuid();
        }
    }
}