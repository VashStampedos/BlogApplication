namespace BlogWebAPI.Entities
{
    public class Subscribe
    {
        public int UserId { get; set; }
        public int SubscriberId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User Subscriber { get; set; } = null!;
    }
}
