namespace BlogWebAPI.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdArticle { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Article Article { get; set; } = null!;
    }
}
