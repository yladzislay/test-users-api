using System.Collections.Generic;
using MongoDB.Driver;
using UsersApi.Domain;

namespace UsersApi.Services
{
    
    public class UserService
    {
        public List<User> GetAll()
        {
            var client = new MongoClient("mongodb://admin:admin@localhost:27017");
            var db = client.GetDatabase("users");
            var collection = db.GetCollection<User>("users");
            return collection
                .Find(c => true)
                .ToList();
        }
    }
}