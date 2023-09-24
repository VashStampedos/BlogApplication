
using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper mapper;
        IWebHostEnvironment _env;
        public BlogController(BlogApplicationContext context, UserManager<User> userManager,IMapper _mapper, IWebHostEnvironment environment)
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
            _env = environment;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogs()
        {
            var blogs = await _db.Blogs.Include(x => x.User).Include(x=>x.Category).Include(x=> x.Articles).ToListAsync();
            List<BlogModel> blogModels = new List<BlogModel>();
            foreach (var blog in blogs)
            {
                var blogModel = mapper.Map<BlogModel>(blog);
                blogModels.Add(blogModel);
            }
            return  Json(blogModels);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserBlogs(int id)
        {
           
            if (id > 0)
            {
                var blogs = await _db.Blogs.Include(x => x.User).Include(x => x.Category).Include(x => x.Articles).Where(x => x.IdUser == id).ToListAsync();
                if (blogs != null)
                {
                    List<BlogModel> blogModels = mapper.Map<List<BlogModel>>(blogs);
                    return Ok(blogModels);
                }
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserBlogs()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var blogs = await _db.Blogs.Include(x => x.User).Include(x=> x.Category).Include(x => x.Articles).Where(x => x.IdUser == user.Id).ToListAsync();
                if (blogs != null)
                {
                    List<BlogModel> blogModels = mapper.Map<List<BlogModel>>(blogs);
                    return Ok(blogModels);
                }
            }
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Categories()
        {
            var categories = await _db.Categories.ToListAsync();
            var categoryModels = mapper.Map<List<CategoryModel>>(categories);
            return Json(categoryModels);
        }

        public record NewBlogRequest(string Name, int idCategory);
        [HttpPost]
        public async Task<IActionResult> AddNewBlog([FromBody] NewBlogRequest request)
        {
            var blogName = request.Name;
            var idBlogCategory = request.idCategory;
            if (!string.IsNullOrEmpty(blogName) && idBlogCategory!= 0 )
            {
                var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == idBlogCategory);
                var user = await _userManager.GetUserAsync(User);
                if (user!=null && category!=null)
                {
                    var blogs = await _db.Blogs.Include(x => x.User).Include(x=> x.Category)
                        .FirstOrDefaultAsync(x => x.IdUser == user.Id && x.Name == blogName && x.IdCategory == category.Id);
                    if(blogs == null)
                    {
                        Blog newBlog = new Blog();
                        newBlog.Name = blogName;
                        newBlog.IdUser = user.Id;
                        newBlog.IdCategory = category.Id;
                        await _db.Blogs.AddAsync(newBlog);
                        await _db.SaveChangesAsync();
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var user =await _userManager.GetUserAsync(User);
            if (id != 0 && user !=null)
            {
                var delBlog =await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id && x.IdUser== user.Id);
                if(delBlog!= null)
                {
                     _db.Blogs.Remove(delBlog);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog =await _db.Blogs.Include(x => x.User).Include(x => x.Category)
                .Include(x => x.Articles).ThenInclude(x=> x.Comments)
                .Include(x=> x.Articles).ThenInclude(x=> x.Likes).FirstAsync(x => x.Id == id);
            if (blog != null)
            {
                var blogModel = mapper.Map<BlogModel>(blog);
                return Json(blogModel);
            }
            return NotFound();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Articles()
        {
            var articles = await _db.Articles.Include(x => x.Blog).ToListAsync();
            if (articles != null){
                
                var articleModels = mapper.Map<List<ArticleModel>>(articles);
                return Json(articleModels);
            }
            return BadRequest("Something went wrong");
           
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await _db.Articles.Include(x => x.Blog).ThenInclude(x=> x.User).FirstOrDefaultAsync(x=> x.Id==id);
            if (article != null)
            {
                var articleModel = mapper.Map<ArticleModel>(article);
               

                return Json(articleModel);

            }
           
           return BadRequest("Article not found");
        }

        public record NewArticleRequest(string Title, string Description, IFormFile? Photo, int IdBlog);
        [HttpPost]
        public async Task<IActionResult> AddNewArticle([FromForm] NewArticleRequest formData)
        //public async Task<IActionResult> AddNewArticle([FromForm] string title, [FromForm] string description, [FromForm] IFormFile photo, [FromForm] int idBlog )
        {
            var title = formData.Title;
            var description = formData.Description;
            var photo = formData.Photo;
            var idBlog = formData.IdBlog;
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || idBlog == 0)
            {
                return BadRequest();
            }
            var check = _db.Blogs.FirstOrDefault(x=> x.Id == idBlog);
            if (check != null)
            {
                if (check.IdUser == user.Id)
                {
                    var result = await _db.Articles.FirstOrDefaultAsync(x => x.Title == title && x.Description == description && x.IdBlog == idBlog);
                    if(result == null)
                    {
                        using (var fs = new FileStream(_env.WebRootPath + "/images/" + photo.FileName, FileMode.Create))
                        {
                            await photo.CopyToAsync(fs);
                        }
                            var newArticle = new Article()
                            {
                                Title = title,
                                Description = description,
                                Photo = ("/images/" + photo.FileName),
                                IdBlog = idBlog
                            };

                        await _db.Articles.AddAsync(newArticle);
                        await _db.SaveChangesAsync();
                        return Ok();
                    }
                }
                
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteArticle(int id)
        { 

            var user = await _userManager.GetUserAsync(User);
            var article = await _db.Articles.Include(x => x.Blog).FirstOrDefaultAsync(x=> x.Id == id);
            if (article != null) 
            {
                if (article.Blog.IdUser == user.Id)
                {
                    try
                    {
                        _db.Articles.Remove(article);
                        _db.SaveChanges();

                    }
                    catch(Exception ex)
                    {
                        var temp = 123;
                    }
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Subscribers()
        {
            var subs = _db.Subscribes.Include(x => x.User).ToList();
            List<SubscribeModel> subModels = new List<SubscribeModel>();
            foreach (var sub in subs)
            {
                var subModel = mapper.Map<SubscribeModel>(sub);
                subModels.Add(subModel);
            }
            return Json(subModels);
        }
        public record LikeResponse(LikeModel[] LikesModel, bool IsLiked);
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLikes(int id)
        {
            if (id != 0)
            {
                bool isLiked = false;
                var likes = await _db.Likes.Include(x => x.User).Include(x => x.Article).Where(x => x.IdArticle == id).ToListAsync();
                var likesModel = mapper.Map<LikeModel[]>(likes);
                if (User.Identity.IsAuthenticated)
                {
                    var user =await _userManager.GetUserAsync(User);
                    var likeCheck = likesModel.FirstOrDefault(x => x.IdUser == user!.Id);
                    if (likeCheck != null)
                    {
                        isLiked = true;
                    }
                }
                return Json(new LikeResponse(likesModel, isLiked));
                
            }
            return BadRequest("Null article id");
        }
        public record LikeRequest(int idArticle);
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                int idUser = user.Id;
                int idArticle = likeRequest.idArticle;

                if (idUser != 0 && idArticle != 0)
                {
                    var checkLike =await _db.Likes.FirstOrDefaultAsync(x=> x.IdUser == idUser && x.IdArticle==idArticle);
                    if (checkLike==null)
                    {
                        Like like = new Like();
                        like.IdUser = idUser;
                        like.IdArticle = idArticle;

                        await _db.Likes.AddAsync(like);
                        await _db.SaveChangesAsync();

                        return Ok();

                    }
                    else
                    {
                        _db.Likes.Remove(checkLike);
                        await _db.SaveChangesAsync();
                        return Ok();
                    }
                }
            }

            return BadRequest("Can not like article");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int id)
        {
            if (id != 0)
            {
                var comments =await _db.Comments.Include(x=> x.User).Include(x=> x.Article).Where(x=> x.IdArticle==id).ToListAsync();
                var commentsModel = mapper.Map<CommentModel[]>(comments);
                return Json(commentsModel);
            }
            return BadRequest("Null article id");
        }

        public record CommentRequest(int idArticle, string description);
        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromBody] CommentRequest commentRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                int idUser = user.Id;
                int idArticle = commentRequest.idArticle;
                string description = commentRequest.description;

                if (idUser != 0 && idArticle != 0 && !string.IsNullOrEmpty(description))
                {
                    Comment newComment = new Comment();
                    newComment.IdUser = idUser;
                    newComment.IdArticle = idArticle;
                    newComment.Description = description;

                    await _db.Comments.AddAsync(newComment);
                    await _db.SaveChangesAsync();

                    return Ok();
                }
            }
            
            return BadRequest("Can not add comment");
        }
        [HttpGet]
        public IActionResult GetBlogsFromSubscribers(int userid)
        {
            var blogs = (from blog in _db.Blogs.Include(x => x.User).Include(x=> x.Category)
                         from d in blog.User.Subscribes
                         where d.SubscriberId == userid
                         select blog).ToList();
            List<BlogModel> blogModels = new List<BlogModel>();
            foreach (var blog in blogs)
            {
                var blogModel = mapper.Map<BlogModel>(blog);
                blogModels.Add(blogModel);
            }
            return Json(blogModels);
        }
        [HttpGet]
        public IActionResult GetArticlesFromBlogsSubscribers(int userid)
        {
            var articles = (from article in _db.Articles.Include(x => x.Blog).ThenInclude(b => b.User)
                            from a in article.Blog.User.Subscribes
                            where a.SubscriberId == userid
                            select article).ToList();
            List<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var article in articles)
            {
                var articleMap = mapper.Map<ArticleModel>(article);
                articleModels.Add(articleMap);
            }
            return Json(articleModels);
        }


    }
}
