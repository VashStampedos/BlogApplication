using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using AutoMapper;

namespace BlogWebAPI.MapperProfiles
{
    public class CategoryMap:Profile
    {
        public CategoryMap()
        {
            CreateMap<Category,CategoryModel>();
        }
    }
}
