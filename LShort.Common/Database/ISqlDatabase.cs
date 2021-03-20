using System;
using System.Data;
using SqlKata;
using SqlKata.Execution;

namespace LShort.Common.Database
{
    /// <summary>
    /// Wrapper around SqlKata QueryFactory, adding a type mapper.
    /// </summary>
    public interface ISqlDatabase
    {
        /// <summary>
        /// Starts constructing a query on the provided table.
        /// </summary>
        /// <param name="table">The name of the table.</param>
        /// <returns>The <see cref="Query"/>.</returns>
        Query Query(string table);

        /// <summary>
        /// Executes a SQL statement.
        /// </summary>
        /// <param name="sql">The SQL statement.</param>
        /// <param name="param">The query params.</param>
        /// <param name="transaction">The DB transaction.</param>
        /// <param name="timeout">The timeout length.</param>
        /// <returns>The number of affected rows.</returns>
        int Statement(string sql, object param = null, IDbTransaction transaction = null, int? timeout = null);

        /// <summary>
        /// Gets the SqlKata <see cref="QueryFactory"/>.
        /// </summary>
        /// <remarks>
        /// This is useful if more complicated actions are required.
        /// </remarks>
        /// <returns>The <see cref="QueryFactory"/>.</returns>
        QueryFactory GetQueryFactory();

        /// <summary>
        /// Maps a type to its corresponding SQL type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>A string representing the equivalent type in SQL.</returns>
        string Map(Type t);
    }
}