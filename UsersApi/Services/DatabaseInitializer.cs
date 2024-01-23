using MongoDB.Driver;
using UsersApi.Models.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersApi.Services
{
    public class DatabaseInitializer
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Position> _positionsCollection;
        private readonly Faker<Position> _positionFaker;
        private readonly Faker<User> _userFaker;

        public DatabaseInitializer(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("users");
            _usersCollection = database.GetCollection<User>("users");
            _positionsCollection = database.GetCollection<Position>("positions");

            _positionFaker = new Faker<Position>()
                .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
                .RuleFor(p => p.Name, f => f.Name.JobTitle())
                .RuleFor(p => p.DefaultSalary, f => f.Random.Decimal(50000, 1500000));

            _userFaker = new Faker<User>()
                .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Login, f => f.Internet.UserName());
        }

        public async Task RunAsync()
        {
            await InitializePositionsAsync();
            await InitializeUsersAsync();
        }

        private async Task InitializePositionsAsync()
        {
            const int requiredPositionsCount = 100;
            var existedPositionsCount = await _positionsCollection.CountDocumentsAsync(FilterDefinition<Position>.Empty);
            if (requiredPositionsCount <= existedPositionsCount) return;

            var countToGenerate = requiredPositionsCount - (int)existedPositionsCount;
            var generatedPositions = _positionFaker.Generate(countToGenerate);

            await _positionsCollection.InsertManyAsync(generatedPositions);
        }

        private async Task InitializeUsersAsync()
        {
            const int requiredUsersCount = 200;
            var existedUsersCount = await _usersCollection.CountDocumentsAsync(FilterDefinition<User>.Empty);
            if (requiredUsersCount <= existedUsersCount) return;
            
            var countToGenerate = requiredUsersCount - (int)existedUsersCount;
            var positions = await _positionsCollection.Find(FilterDefinition<Position>.Empty).ToListAsync();
            var generatedUsers = _userFaker.RuleFor(u => u.PositionId, f => GetRandomPositionId(positions)).Generate(countToGenerate);

            await _usersCollection.InsertManyAsync(generatedUsers);
        }

        private static string GetRandomPositionId(List<Position> positions)
        {
            var random = new Random();
            var randomIndex = random.Next(positions.Count);
            return positions[randomIndex].Id;
        }
    }
}
