using System.Text.Json.Serialization;

namespace BlogWebAPI.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdUser { get; set; }
        public int IdCategory { get; set; }
        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
       
        public virtual ICollection<Article>? Articles { get; set; } = null!;
    }
}
