using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using LShort.Common.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace LShort.Common.Database.Implementation
{
    public class MongoDatabaseConnectivityProvider : IDatabaseConnectivityProvider<IMongoDatabase>
    {
        public IMongoDatabase Connect(string filePath)
        {
            var creds = ReadDatabaseCredentials(filePath);
            
            // had issues with docker not being able to connect to mongo via hostname (host.docker.internal or container name)
            // solution: resolve to IP address and connect that way
            // https://stackoverflow.com/a/61383617/13506648
            creds.Host = Dns.GetHostAddresses(creds.Host)
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            
            var mongoClient = new MongoClient(
                $"mongodb://{creds.Username}:{creds.Password}@{creds.Host}/{creds.Database}");

            return mongoClient.GetDatabase(creds.Database);
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