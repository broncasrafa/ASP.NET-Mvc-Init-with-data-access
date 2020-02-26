using MongoDB.Driver;

namespace DataAccess.Providers.MongoDB.Interface
{
    public interface IMongoContext
    {
        IMongoDatabase GetDatabase();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
