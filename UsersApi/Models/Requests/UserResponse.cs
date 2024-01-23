using System.Collections.Generic;
using UsersApi.Models.DTO;

namespace UsersApi.Models.Requests
{
    public class UserResponse
    {
        public int Total { get; set; }
        public List<UserDto> Users { get; set; }
    }
}