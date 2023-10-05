using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using BlogWebAPI.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogWebAPI.Services
{
    public class BlogService
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper mapper;
        IWebHostEnvironment _env;

        public BlogService(
            BlogApplicationContext context,
            IMapper _mapper,
            UserManager<User> userManager,
            IWebHostEnvironment env
            )
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
            _env = env;
        }

        public async Task<IEnumerable<BlogModel>> GetBlogsAsync()
        {
            var blogs = await _db.Blogs.Include(x => x.User).Include(x => x.Category).Include(x => x.Articles).ToListAsync();
            var blogModels = mapper.Map<IEnumerable<BlogModel>>(blogs);
            return blogModels;
        }

        public async Task<IEnumerable<BlogModel>> GetUserBlogsByIdAsync(int userId)
        {
            var blogs = await _db.Blogs.Include(x => x.User).Include(x => x.Category).Include(x => x.Articles).Where(x => x.IdUser == userId).ToListAsync();
            var blogModels = mapper.Map<IEnumerable<BlogModel>>(blogs);
            return blogModels;
            
        }

        public async Task<IEnumerable<BlogModel>> GetCurrentUserBlogsAsync(ClaimsPrincipal user)
        { 
            var currentUser =await _userManager.GetUserAsync(user);
            var blogs = await GetUserBlogsByIdAsync(currentUser.Id);
            return blogs;

        }


        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            var categoryModels = mapper.Map<List<CategoryModel>>(categories);
            return categoryModels;

        }

        private Blog InitializeBlog(string name, int userId, int categoryId)
        {
            
            Blog newBlog = new Blog();
            newBlog.Name = name;
            newBlog.IdUser = userId;
            newBlog.IdCategory = categoryId;
            return newBlog;
        }


        public async Task<bool> CreateNewBlogAsync(string name, int categoryId, ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            var category = await _db.Categories.FirstOrDefaultAsync(x=> x.Id == categoryId);
            if(currentUser != null && category != null)
            {
                var blogs = await _db.Blogs.Include(x => x.User).Include(x => x.Category)
                            .FirstOrDefaultAsync(x => x.IdUser == currentUser.Id && x.Name == name && x.IdCategory == category.Id);
                if(blogs == null)
                {
                    var newBlog = InitializeBlog(name, currentUser.Id, category.Id);
                    await _db.Blogs.AddAsync(newBlog);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteBlogAsync(int id, int userId)
        {
            var delBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id && x.IdUser == userId);
            if (delBlog != null)
            {
                _db.Blogs.Remove(delBlog);
                await _db.SaveChangesAsync();
                return true;
                
            }
            return false;
        }

        public async Task<BlogModel> GetBlogAsync(int id)
        {
            var blog = await _db.Blogs.Include(x => x.User).Include(x => x.Category)
              .Include(x => x.Articles).ThenInclude(x => x.Comments)
              .Include(x => x.Articles).ThenInclude(x => x.Likes).FirstOrDefaultAsync(x => x.Id == id);
            
            var blogModel = mapper.Map<BlogModel>(blog);
            return blogModel;
        }

        private Article InitializeArticle(string title, string description, string photoFileName, int blogId)
        {

            Article article = new Article();
            article.Title = title;
            article.Description = description;
            article.Photo = ("/images/" + photoFileName);
            article.IdBlog = blogId;
            return article;
        }

        private async Task SavePhotoAsync(IFormFile photo)
        {
            using (var fs = new FileStream(_env.WebRootPath + "/images/" + photo.FileName, FileMode.Create))
            {
                await photo.CopyToAsync(fs);
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesAsync()
        {
            var articles = await _db.Articles.Include(x => x.Blog).ToListAsync();
            var articleModels = mapper.Map<IEnumerable<ArticleModel>>(articles);
            return articleModels;
        }

        public async Task<ArticleModel> GetArticleAsync(int id)
        {
            var article = await _db.Articles.Include(x => x.Blog).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            var articleModels = mapper.Map<ArticleModel>(article);
            return articleModels;
        }

        public async Task<bool> CreateArticleAsync(string title, string description, IFormFile photo, int blogId, User user)
        {
            
            var blogArticle =await _db.Blogs.FirstOrDefaultAsync(x => x.Id == blogId);
            if (blogArticle != null)
            {
                if(blogArticle.IdUser == user.Id)
                {
                    var checkArticle = await _db.Articles.FirstOrDefaultAsync(x => x.Title == title && x.Description == description && x.IdBlog == blogId);
                    if (checkArticle == null)
                    {
                        await SavePhotoAsync(photo);
                        var newArticle = InitializeArticle(title, description, photo.FileName, blogId);
                        await _db.Articles.AddAsync(newArticle);
                        await _db.SaveChangesAsync();
                        return true;

                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteArticleAsync(int id, int userId)
        {
            var delArticle = await _db.Articles.Include(x => x.Blog).FirstOrDefaultAsync(x => x.Id == id);
            if (delArticle != null)
            {
                if(delArticle.Blog.IdUser == userId)
                {
                    _db.Articles.Remove(delArticle);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<LikeResult> GetLikesAsync(int id, int userId)
        {
            bool isLiked = false;
            var likes = await _db.Likes.Include(x => x.User).Include(x => x.Article).Where(x => x.IdArticle == id).ToListAsync();
            var likesModel = mapper.Map<LikeModel[]>(likes);

            var likeCheck = likesModel.FirstOrDefault(x => x.IdUser == userId);
            if (likeCheck != null)
            {
                isLiked = true;
            }
            return new LikeResult(likesModel, isLiked);
        }

        public async Task AddLikeAsync(int id, int userId)
        {
            var checkLike = await _db.Likes.FirstOrDefaultAsync(x => x.IdUser == userId && x.IdArticle == id);
            if (checkLike == null)
            {
                Like like = new Like();
                like.IdUser = userId;
                like.IdArticle = id;

                await _db.Likes.AddAsync(like);
                await _db.SaveChangesAsync(); 
            }
            else
            {
                _db.Likes.Remove(checkLike);
                await _db.SaveChangesAsync();
                
            }

        }

        public async Task<IEnumerable<CommentModel>> GetCommentsAsync(int id)
        {
            var comments = await _db.Comments.Include(x => x.User).Include(x => x.Article).Where(x => x.IdArticle == id).ToListAsync();
            var commentsModel = mapper.Map<CommentModel[]>(comments);
            return commentsModel;
        }

        private Comment InitializeComment(int articleId, string description, int userId)
        {
            Comment comment = new Comment();
            comment.IdArticle = articleId;
            comment.Description = description;  
            comment.IdUser = userId;
            return comment;
        }

        public async Task CreateCommentAsync(int id, string description, int userId)
        {
            var comment = InitializeComment(id, description, userId);
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }
    }
}
