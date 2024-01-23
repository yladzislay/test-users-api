using Microsoft.AspNetCore.Mvc;
using UsersApi.Models.Requests;
using UsersApi.Services;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UsersController(MongoDbService mongoDbService) : ControllerBase
    {
        [Route("list")]
        public IActionResult GetList()
        {
            var response = new UserResponse
            {
                Users = mongoDbService.GetAllUsers(),
            };

            response.Total = response.Users.Count;

            return Ok(response);
        }
    }
}