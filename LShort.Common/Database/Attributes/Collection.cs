using System;

namespace LShort.Common.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Collection : Attribute
    {
        /// <summary>
        /// The name of the collection.
        /// </summary>
        public string Name { get; }

        public Collection(string name)
        {
            this.Name = name;
        }
    }
}