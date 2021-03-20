using System;

namespace LShort.Common.Database
{
    /// <summary>
    /// Maps types to a string representing
    /// their equivalent type in SQL.
    /// </summary>
    public interface ISqlTypeMapper
    {
        /// <summary>
        /// Maps a type to its corresponding SQL type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>A string representing the equivalent type in SQL.</returns>
        string Map(Type t);
    }
}