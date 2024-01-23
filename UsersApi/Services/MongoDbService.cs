using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UsersApi.Models.DTO;
using UsersApi.Models.Entities;

namespace UsersApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Position> _positionsCollection;

        public MongoDbService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("users");
            _usersCollection = database.GetCollection<User>("users");
            _positionsCollection = database.GetCollection<Position>("positions");
        }

        public List<UserDto> GetAllUsers()
        {
            var allUsers = _usersCollection.AsQueryable()
                .Join(
                    _positionsCollection.AsQueryable(),
                    user => user.PositionId,
                    position => position.Id,
                    (user, position) => new UserDto
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Name = user.Name,
                        DefaultSalary = position.DefaultSalary,
                        Position = position.Name
                    }
                )
                .ToList();

            return allUsers;
        }
    }
}