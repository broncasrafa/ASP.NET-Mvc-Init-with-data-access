using System.Configuration;
using DataAccess.Providers.MongoDB.Interface;
using MongoDB.Driver;

namespace DataAccess.Providers.MongoDB
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public MongoClient MongoClient { get; set; }
        private string _DatabaseName
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("MongoDBDatabaseName");
            }
        }


        public IMongoDatabase GetDatabase()
        {
            var connectionString = GetConnectionString();

            MongoClient = new MongoClient(connectionString);

            return MongoClient.GetDatabase(_DatabaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return Database.GetCollection<T>(name);
        }

        
        private void ConfigureMongo()
        {
            if (MongoClient != null)
                return;

            Database = GetDatabase();
        }
        private string GetConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MongoDBConnectionString").Replace("{DATABASE_NAME}", _DatabaseName);
        }
    }
}
