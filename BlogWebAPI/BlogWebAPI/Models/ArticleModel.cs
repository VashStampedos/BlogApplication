using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int IdBlog { get; set; }
        public BlogModel Blog { get; set; } = null!;
        ////TODO
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
    }
}
