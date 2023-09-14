using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int IdUser { get; set; }
        public int IdArticle { get; set; }

        public UserModel User { get; set; } = null!;
        public ArticleModel Article { get; set; } = null!;
    }
}
