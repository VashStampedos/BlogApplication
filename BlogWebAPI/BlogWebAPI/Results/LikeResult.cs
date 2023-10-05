using BlogWebAPI.Models;

namespace BlogWebAPI.Results
{
    
    public class LikeResult
    {
        public IEnumerable<LikeModel> LikesModel { get; set; }
        public bool IsLiked { get; set; }

        public LikeResult(IEnumerable<LikeModel> likesModel, bool isLiked)
        {
            LikesModel = likesModel;
            IsLiked = isLiked;
        }
    }
}
