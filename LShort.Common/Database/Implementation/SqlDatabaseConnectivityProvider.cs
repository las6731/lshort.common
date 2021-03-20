using System;
using System.IO;
using LShort.Common.Models;
using MongoDB.Bson.Serialization;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace LShort.Common.Database.Implementation
{
    public class SqlDatabaseConnectivityProvider : IDatabaseConnectivityProvider<ISqlDatabase>
    {
        public ISqlDatabase Connect(string filePath)
        {
            var creds = ReadDatabaseCredentials(filePath);

            var connection =
                new NpgsqlConnection(
                    $"Host={creds.Host};Username={creds.Username};Password={creds.Password};Database={creds.Database};");

            var queryFactory = new QueryFactory(connection, new PostgresCompiler());

            return new SqlDatabase(queryFactory, new SqlTypeMapper());
        }
        
        private DatabaseCredentials ReadDatabaseCredentials(string filePath)
        {
            FileStream fileStream = null;
            StreamReader streamReader = null;
            
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open);
                streamReader = new StreamReader(fileStream);
                return BsonSerializer.Deserialize<DatabaseCredentials>(streamReader.ReadToEnd());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Database connection credentials file not found.");
                return null;
            }
            finally
            {
                streamReader?.Close();
                fileStream?.Close();
            }
        }
    }
}