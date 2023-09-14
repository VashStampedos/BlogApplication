using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class SubscribeMap:Profile
    {
        public SubscribeMap()
        {
            CreateMap<Subscribe, SubscribeModel>();
        }
    }
}
