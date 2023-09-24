using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<BlogModel> Blogs { get; set; } = null!;
        public ICollection<LikeModel> Likes { get; set; } = null!;
        public ICollection<CommentModel> Comments { get; set; } = null!;
        public ICollection<SubscribeModel> Subscribes { get; set; } = null!;
    }
}

