
using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BlogWebAPI.Services;

using BlogWebAPI.DTO.Blog;
using BlogWebAPI.Results;
using BlogWebAPI.Storages;
using System.Net;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        UserManager<User> userManager;
        BlogService blogService;
        ValidatorsStorage validators;
       
        public BlogController( 
            UserManager<User> userManager,
            BlogService blogService,
            ValidatorsStorage validators
            )
        {
            this.userManager = userManager;
            this.blogService = blogService;
            this.validators = validators;
           
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogs()
        {
            var blogs =await blogService.GetBlogsAsync();
            return  Ok(ApiResult<IEnumerable<BlogModel>>.Success(blogs));
        }
        [HttpGet]
        public async Task<IActionResult> GetUserBlogs(int id)
        {
            if (id > 0)
            {
                var blogs = await blogService.GetUserBlogsByUserIdAsync(id); 
                return Ok(ApiResult<IEnumerable<BlogModel>>.Success(blogs));
                
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserBlogs()
        {
            if (User.Identity.IsAuthenticated)
            {
                var blogs = await blogService.GetCurrentUserBlogsAsync(User);
                return Ok(ApiResult<IEnumerable<BlogModel>>.Success(blogs));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Categories()
        {
            var categories = await blogService.GetCategoriesAsync();
            return Ok(ApiResult<IEnumerable<CategoryModel>>.Success(categories));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBlog([FromBody] CreateBlogRequest request)
        {
            if (ModelState.IsValid)
            {
                var createResult = await blogService.CreateNewBlogAsync(request.Name, request.CategoryId, User);
                
                    return Ok(ApiResult<int>.Success(createResult));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBlog(int id)
        {
           
            if (id > 0 )
            {
                var deleteResult = await blogService.DeleteBlogAsync(id, User);
                return Ok(ApiResult<int>.Success(deleteResult));
                
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlog(int id)
        {
            if (id > 0)
            {
                var blogModel =await blogService.GetBlogModelAsync(id);
                return Ok(ApiResult<BlogModel>.Success(blogModel));

                
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Articles()
        {   
            var articleModels = await blogService.GetArticlesAsync();
            return Ok(ApiResult<IEnumerable<ArticleModel>>.Success(articleModels));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticle(int id)
        {
            if(id > 0)
            {
                var articleModel =await blogService.GetArticleModelByIdAsync(id);
                return Ok(ApiResult<ArticleModel>.Success(articleModel));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }

        
        [HttpPost]
        public async Task<IActionResult> AddNewArticle([FromForm] CreateArticleRequest request)
        {
            if (ModelState.IsValid)
            {
               
                var createResult = await blogService.CreateArticleAsync(request.Title, request.Description, request.Photo, request.BlogId, User);
                
                return Ok(ApiResult<int>.Success(createResult));

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteArticle(int id)
        { 
            if(id > 0)
            {
                var deleteResult = await blogService.DeleteArticleAsync(id, User);
                
                return Ok(ApiResult<int>.Success(deleteResult));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }

        
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLikes(int id)
        {
            if (id > 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    
                    var likeResult = await blogService.GetLikesAsync(id, User);
                    return Ok(ApiResult<LikeResult>.Success(likeResult));
                }
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            if(likeRequest.idArticle > 0 )
            {
                
                await blogService.AddLikeAsync(likeRequest.idArticle, User);
                return Ok(ApiResult<string>.Success("Succees"));
                
            }

            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int id)
        {
            if (id > 0)
            {
                var comments = await blogService.GetCommentsAsync(id);
                return Ok(ApiResult<IEnumerable<CommentModel>>.Success(comments));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request" }));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromBody] CreateCommentRequest commentRequest)
        {
          
            if(ModelState.IsValid)
            {
                
                await blogService.CreateCommentAsync(commentRequest.ArticleId, commentRequest.Description, User);
                return Ok(ApiResult<string>.Success("Succees")) ;
                
            }
            
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid Request"}));
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
