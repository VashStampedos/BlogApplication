using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class UserMap:Profile
    {
        public UserMap()
        {
            CreateMap<User, UserModel>();
               
        }
    }
}
