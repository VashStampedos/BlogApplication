using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;

namespace BlogWebAPI.MapperProfiles
{
    public class CommentMap:Profile
    {
        public CommentMap()
        {
            CreateMap<Comment, CommentModel>();
        }
    }
}
