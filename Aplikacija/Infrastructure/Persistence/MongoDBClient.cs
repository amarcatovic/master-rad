using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public interface IMongoDBClient
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }

    public class MongoDBClient : IMongoDBClient
    {
        private readonly IMongoDatabase _database;

        public MongoDBClient(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
