using BlogWebAPI.Validators.RequestsValidators.AuthValidators;
using BlogWebAPI.Validators.RequestsValidators.BlogValidators;

namespace BlogWebAPI.Storages
{
    
    public class ValidatorsStorage
    {
        public CreateCommentValidator _createCommentValidator { get; set; }
        public CreateBlogValidator _createBlogValidator { get; set; }
        public CreateArticleValidator _createArticleValidator { get; set; }
        public RegisterUserValidator _registerValidator { get; set; }
        public ConfirmEmailValidator _confirmEmailValidator { get; set; }
        public LoginUserValidator _logInValidator { get; set; }
        
        
        public ValidatorsStorage(
            CreateCommentValidator createCommentValidator,
            CreateBlogValidator createBlogValidator,
            CreateArticleValidator createArticleValidator,
            RegisterUserValidator registerValidator,
            ConfirmEmailValidator confirmEmailValidator,
            LoginUserValidator logInValidator
            )
        {
            _createCommentValidator = createCommentValidator;
            _createBlogValidator = createBlogValidator;
            _createArticleValidator = createArticleValidator;
            _registerValidator = registerValidator;
            _confirmEmailValidator = confirmEmailValidator;
            _logInValidator = logInValidator;


        }
    }
}
