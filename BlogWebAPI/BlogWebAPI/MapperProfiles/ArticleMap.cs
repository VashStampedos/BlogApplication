using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class ArticleMap:Profile
    {
        public ArticleMap()
        {
            CreateMap<Article, ArticleModel>();


        }
    }
}
