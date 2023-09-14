using System.Text.Json.Serialization;

namespace BlogWebAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
