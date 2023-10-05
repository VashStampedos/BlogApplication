using BlogWebAPI.Models;

namespace BlogWebAPI.DTO.User
{
    public class UserResponse
    {
        public UserModel UserModel { get; set; }
        public bool IsSubscribe { get; set; }
       
    }
}
