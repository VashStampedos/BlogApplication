using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class SubscribeModel
    {
        public int UserId { get; set; }
        public int SubscriberId { get; set; }

        public UserModel User { get; set; } = null!;
        public UserModel Subscriber { get; set; } = null!;
    }
}
