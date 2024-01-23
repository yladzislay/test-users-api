using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using UsersApi.Models.Entities;
using UsersApi.Services;

namespace UsersApi
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:3004")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            BsonClassMap.RegisterClassMap<User>(m => m.AutoMap());
            BsonClassMap.RegisterClassMap<Position>(m => m.AutoMap());

            var mongoDbConnectionString = Configuration.GetSection("MongoDB:ConnectionString").Value;
            var mongoClient = new MongoClient(mongoDbConnectionString);
            services.AddSingleton(mongoClient);
            services.AddSingleton<MongoDbService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}