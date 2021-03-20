using System;
using System.Dynamic;

namespace LShort.Common.Database.Implementation
{
    public class SqlTypeMapper : ISqlTypeMapper
    {
        public string Map(Type t)
        {
            return t.Name switch
            {
                nameof(DateTime) => "timestamptz",
                nameof(Int32) => "int",
                nameof(Boolean) => "bool",
                nameof(Guid) => "uuid",
                nameof(Double) => "double precision",
                nameof(ExpandoObject) => "json",
                _ => "text"
            };
        }
    }
}