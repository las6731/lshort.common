using System;

namespace LShort.Common.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Table : Attribute
    {
        /// <summary>
        /// The name of the collection.
        /// </summary>
        public string Name { get; }

        public Table(string name)
        {
            this.Name = name;
        }
    }
}