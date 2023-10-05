namespace BlogWebAPI.DTO.Blog
{
    public class CreateArticleRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public int BlogId { get; set; }
    }
}
