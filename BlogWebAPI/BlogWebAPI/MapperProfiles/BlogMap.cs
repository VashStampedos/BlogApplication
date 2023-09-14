using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class BlogMap:Profile
    {
        public BlogMap()
        {
            CreateMap<Blog, BlogModel>();
        }
    }
}
