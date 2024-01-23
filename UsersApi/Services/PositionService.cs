using MongoDB.Driver;
using UsersApi.Domain;

namespace UsersApi.Services
{
    public class PositionService
    {
        public Position GetById(string id)
        {
            var client = new MongoClient("mongodb://admin:admin@localhost:27017");
            var db = client.GetDatabase("users");
            var collection = db.GetCollection<Position>("positions");
            return collection
                .Find(p => p.Id == id)
                .FirstOrDefault();
        }
    }
}