using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogModel> Blogs { get; set; }
    }
}
