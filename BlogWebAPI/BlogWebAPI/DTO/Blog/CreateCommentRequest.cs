namespace BlogWebAPI.DTO.Blog
{
    public class CreateCommentRequest
    {
        public int ArticleId { get; set; }
        public string Description { get; set; }
    }
}
