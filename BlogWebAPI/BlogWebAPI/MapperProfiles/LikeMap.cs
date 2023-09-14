using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class LikeMap:Profile
    {
        public LikeMap()
        {
            CreateMap<Like, LikeModel>();  
        }
    }
}
