
using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogWebAPI.Services;
using FluentValidation;
using BlogWebAPI.DTO.Auth;
using BlogWebAPI.DTO.Blog;
using BlogWebAPI.Validators.RequestsValidators.BlogValidators;
using BlogWebAPI.Results;
using BlogWebAPI.Storages;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper mapper;
        IWebHostEnvironment _env;
        BlogService _blogService;
        ValidatorsStorage _validators;
       
        public BlogController(BlogApplicationContext context, 
            UserManager<User> userManager,
            IMapper _mapper, 
            IWebHostEnvironment environment,
            BlogService blogService,
            ValidatorsStorage validators
            )
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
            _env = environment;
            _blogService = blogService;
            _validators = validators;
           
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogs()
        {
            var blogs =await _blogService.GetBlogsAsync();
            return  Json(blogs);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserBlogs(int id)
        {
            if (id > 0)
            {
                var blogs = await _blogService.GetUserBlogsByIdAsync(id); 
                return Json(blogs);
                
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserBlogs()
        {
            if (User.Identity.IsAuthenticated)
            {
                var blogs = await _blogService.GetCurrentUserBlogsAsync(User);
                return Json(blogs);
            }
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Categories()
        {
            var categories = await _blogService.GetCategoriesAsync();
            return Json(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBlog([FromBody] CreateBlogRequest request)
        {
            var result =await _validators._createBlogValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var createResult = await _blogService.CreateNewBlogAsync(request.Name, request.CategoryId, User);
                if(createResult)
                    return Ok();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var user =await _userManager.GetUserAsync(User);
           
            if (id > 0 && user !=null)
            {
                var deleteResult = await _blogService.DeleteBlogAsync(id, user.Id);
                if (deleteResult)
                return Ok();
                
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlog(int id)
        {
            if (id > 0)
            {
                var blogModel =await _blogService.GetBlogAsync(id);
                if (blogModel != null)
                {
                    return Json(blogModel);

                }
            }
            return NotFound();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Articles()
        {   
            var articleModels = await _blogService.GetArticlesAsync();
            return Json(articleModels);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticle(int id)
        {
            if(id > 0)
            {
                var articleModel =await _blogService.GetArticleAsync(id);
                return Json(articleModel);
            }
            return BadRequest("Article not found");
        }

        
        [HttpPost]
        public async Task<IActionResult> AddNewArticle([FromForm] CreateArticleRequest request)
        {
            var result =await _validators._createArticleValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var createResult = await _blogService.CreateArticleAsync(request.Title, request.Description, request.Photo, request.BlogId, user);
                if (createResult)
                    return Ok();

            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteArticle(int id)
        { 
            if(id > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var deleteResult = await _blogService.DeleteArticleAsync(id, user.Id);
                if(deleteResult) 
                    return Ok();
            }
            return BadRequest();
        }

        
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLikes(int id)
        {
            if (id > 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user =await _userManager.GetUserAsync(User);
                    var likeResult = await _blogService.GetLikesAsync(id, user.Id);
                    return Json(likeResult);
                }
            }
            return BadRequest();
        }
        public record LikeRequest(int idArticle);
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            if(likeRequest.idArticle > 0 )
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _blogService.AddLikeAsync(likeRequest.idArticle, user.Id);
                    return Ok();
                }
            }

            return BadRequest("Can not like article");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int id)
        {
            if (id > 0)
            {
                var comments = await _blogService.GetCommentsAsync(id);
                return Json(comments);
            }
            return BadRequest("Wrong article id");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromBody] CreateCommentRequest commentRequest)
        {
            var result =await _validators._createCommentValidator.ValidateAsync(commentRequest);
            if(result.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                        await _blogService.CreateCommentAsync(commentRequest.ArticleId, commentRequest.Description, user.Id);

                        return Ok();
               
                }
            }
            
            return BadRequest("Can not add comment");
        }

        //[HttpGet]
        //public IActionResult Subscribers()
        //{
        //    var subs = _db.Subscribes.Include(x => x.User).ToList();
        //    List<SubscribeModel> subModels = new List<SubscribeModel>();
        //    foreach (var sub in subs)
        //    {
        //        var subModel = mapper.Map<SubscribeModel>(sub);
        //        subModels.Add(subModel);
        //    }
        //    return Json(subModels);
        //}
        //[HttpGet]
        //public IActionResult GetBlogsFromSubscribers(int userid)
        //{
        //    var blogs = (from blog in _db.Blogs.Include(x => x.User).Include(x=> x.Category)
        //                 from d in blog.User.Subscribes
        //                 where d.SubscriberId == userid
        //                 select blog).ToList();
        //    List<BlogModel> blogModels = new List<BlogModel>();
        //    foreach (var blog in blogs)
        //    {
        //        var blogModel = mapper.Map<BlogModel>(blog);
        //        blogModels.Add(blogModel);
        //    }
        //    return Json(blogModels);
        //}
        //[HttpGet]
        //public IActionResult GetArticlesFromBlogsSubscribers(int userid)
        //{
        //    var articles = (from article in _db.Articles.Include(x => x.Blog).ThenInclude(b => b.User)
        //                    from a in article.Blog.User.Subscribes
        //                    where a.SubscriberId == userid
        //                    select article).ToList();
        //    List<ArticleModel> articleModels = new List<ArticleModel>();
        //    foreach (var article in articles)
        //    {
        //        var articleMap = mapper.Map<ArticleModel>(article);
        //        articleModels.Add(articleMap);
        //    }
        //    return Json(articleModels);
        //}


    }
}
