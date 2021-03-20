using System;
using System.Data;
using SqlKata;
using SqlKata.Execution;

namespace LShort.Common.Database.Implementation
{
    public class SqlDatabase : ISqlDatabase
    {
        private QueryFactory db;
        private ISqlTypeMapper typeMapper;

        public SqlDatabase(QueryFactory db, ISqlTypeMapper typeMapper)
        {
            this.db = db;
            this.typeMapper = typeMapper;
        }
        
        public Query Query(string table)
        {
            return db.Query(table);
        }

        public int Statement(string sql, object param, IDbTransaction transaction, int? timeout)
        {
            return db.Statement(sql, param, transaction, timeout);
        }

        public QueryFactory GetQueryFactory()
        {
            return db;
        }

        public string Map(Type t)
        {
            return typeMapper.Map(t);
        }
    }
}