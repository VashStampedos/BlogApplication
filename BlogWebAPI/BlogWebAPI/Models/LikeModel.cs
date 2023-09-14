using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class LikeModel
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdArticle { get; set; }

        public UserModel User { get; set; } = null!;
        public ArticleModel Article { get; set; } = null!;
    }
}
