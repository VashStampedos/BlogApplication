using AutoMapper;
using BlogWebAPI.Domain.Exceptions;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using BlogWebAPI.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogWebAPI.Domain.Services
{
    public class BlogService: ApplicationDbContextService
    {
        IMapper mapper;
        IWebHostEnvironment env;

        public BlogService(
            BlogApplicationContext context,
            IMapper mapper,
            UserManager<User> userManager,
            IWebHostEnvironment env
            ):base(context, userManager)
        {
            this.mapper = mapper;
            this.env = env;
        }

        public async Task<IEnumerable<BlogModel>> GetBlogsAsync()
        {
            var blogs = await context.Blogs.AsNoTracking().Include(x => x.User).Include(x => x.Category).Include(x => x.Articles).ToListAsync();
            var blogModels = mapper.Map<IEnumerable<BlogModel>>(blogs);
            return blogModels;
        }

        public async Task<IEnumerable<BlogModel>> GetUserBlogsByUserIdAsync(int userId)
        {
            var blogs = await context.Blogs.Include(x => x.User).Include(x => x.Category).Include(x => x.Articles).Where(x => x.IdUser == userId).ToListAsync();
            var blogModels = mapper.Map<IEnumerable<BlogModel>>(blogs);
            return blogModels;
            
        }

        public async Task<IEnumerable<BlogModel>> GetCurrentUserBlogsAsync(ClaimsPrincipal user)
        {
            var currentUser = await GetCurrentUserAsync(user);
            var blogs = await GetUserBlogsByUserIdAsync(currentUser.Id);
            return blogs;

        }


        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            var categories = await context.Categories.ToListAsync();
            var categoryModels = mapper.Map<List<CategoryModel>>(categories);
            return categoryModels;

        }



        public async Task<int> CreateNewBlogAsync(string name, int categoryId, ClaimsPrincipal user)
        {
            var currentUser = await GetCurrentUserAsync(user);
            await CheckCategoryAsync(categoryId);
            await CheckExistedBlogAsync(currentUser.Id, categoryId, name);
            var newBlog = InitializeBlog(name, currentUser.Id, categoryId);
            await context.Blogs.AddAsync(newBlog);
            await SaveChangesAsync();
            return newBlog.Id;
        }

       
        public async Task<int> DeleteBlogAsync(int id, ClaimsPrincipal user)
        {
            var currentUser = await GetCurrentUserAsync(user);
            var delBlog = await GetUserBlogAsync(id, currentUser.Id);
            context.Blogs.Remove(delBlog);
            await SaveChangesAsync();
            return delBlog.Id;
        }

       

        public async Task<BlogModel> GetBlogModelAsync(int id)
        {
            var blog = await GetBlogAsync(id);
            var blogModel = mapper.Map<BlogModel>(blog);
            return blogModel;
        }

        public async Task<Blog> GetBlogAsync(int id)
        {
            var blog = await context.Blogs.Include(x => x.User).Include(x => x.Category)
              .Include(x => x.Articles).ThenInclude(x => x.Comments)
              .Include(x => x.Articles).ThenInclude(x => x.Likes).FirstOrDefaultAsync(x => x.Id == id);
            _ = blog ?? throw new NotFoundException("Blog not found");
            return blog;
        }


        public async Task<IEnumerable<ArticleModel>> GetArticlesAsync()
        {
            var articles = await context.Articles.Include(x => x.Blog).ToListAsync();
            var articleModels = mapper.Map<IEnumerable<ArticleModel>>(articles);
            return articleModels;
        }

        public async Task<ArticleModel> GetArticleModelByIdAsync(int id)
        {
            var article = await GetArticleByIdAsync(id);
            var articleModels = mapper.Map<ArticleModel>(article);
            return articleModels;
        }

       

        public async Task<int> CreateArticleAsync(string title, string description, IFormFile photo, int blogId, ClaimsPrincipal currentUser)
        {
            var blogArticle =await GetBlogAsync(blogId);
            var user = await GetCurrentUserAsync(currentUser);
            CheckUserBlog(blogArticle, user.Id);
            await CheckArticle(title, description, blogId);
            await SavePhotoAsync(photo);
            var newArticle = InitializeArticle(title, description, photo.FileName, blogId);
            await context.Articles.AddAsync(newArticle);
            await SaveChangesAsync();
            return newArticle.Id;
        }
        
        public async Task<int> DeleteArticleAsync(int id, ClaimsPrincipal currentUser)
        {
            var user = await GetCurrentUserAsync(currentUser);
            var delArticle = await GetArticleByIdAsync(id);
            CheckUserBlog(delArticle.Blog, user.Id); 
            context.Articles.Remove(delArticle);
            await context.SaveChangesAsync();
            return delArticle.Id;
        }

        public async Task<LikeResult> GetLikesAsync(int id, ClaimsPrincipal user)
        {
            bool isLiked = false;
            var currentUser = await GetCurrentUserAsync(user);  
            var likes = await context.Likes.Include(x => x.User).Include(x => x.Article).Where(x => x.IdArticle == id).ToListAsync();
            var likesModel = mapper.Map<LikeModel[]>(likes);

            var likeCheck = likesModel.FirstOrDefault(x => x.IdUser == currentUser.Id);
            if (likeCheck != null)
            {
                isLiked = true;
            }
            return new LikeResult(likesModel, isLiked);
        }

        public async Task AddLikeAsync(int id, ClaimsPrincipal currentUser)
        {
            var user = await GetCurrentUserAsync(currentUser);
            var checkLike = await context.Likes.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.IdArticle == id);
            if (checkLike == null)
            {
                Like like = new Like();
                like.IdUser = user.Id;
                like.IdArticle = id;

                await context.Likes.AddAsync(like);
                await SaveChangesAsync(); 
            }
            else
            {
                context.Likes.Remove(checkLike);
                await SaveChangesAsync();
                
            }

        }

        public async Task<IEnumerable<CommentModel>> GetCommentsAsync(int id)
        {
            var comments = await context.Comments.Include(x => x.User).Include(x => x.Article).Where(x => x.IdArticle == id).ToListAsync();
            
            var commentsModel = mapper.Map<CommentModel[]>(comments);
            return commentsModel;
        }
        public async Task CreateCommentAsync(int articleId, string description, ClaimsPrincipal user)
        {
            var currentUser = await GetCurrentUserAsync(user);
            var comment = InitializeComment(articleId, description, currentUser.Id);
            await context.Comments.AddAsync(comment);
            await SaveChangesAsync();
        }
        private async Task<Blog> GetUserBlogAsync(int id, int userId)
        {
            var blog = await context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.IdUser == userId);
            _ = blog ?? throw new NotFoundException("Blog not found");
            return blog;
        }
        private async Task CheckArticle(string title, string description, int blogId)
        {
            var checkArticle = await context.Articles.AsNoTracking().AnyAsync(x => x.Title == title && x.Description == description && x.IdBlog == blogId);
            if (checkArticle)
                throw new ConflictException("Article is already exists");
        }

        private void CheckUserBlog(Blog blog, int userId)
        {
            if (blog.IdUser != userId)
                throw new ConflictException("Not blog of this user");
        }

        private async Task<Article> GetArticleByIdAsync(int id)
        {
            var article = await context.Articles.Include(x => x.Blog).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            _ = article ?? throw new NotFoundException("Article not found");
            return article;
        }
        private async Task<Blog> GetUserBlogByIdAsync(int id)
        {
            var blog = await context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            _ = blog ?? throw new NotFoundException("Blog not found");
            return blog;
        }

        private Comment InitializeComment(int articleId, string description, int userId)
        {
            Comment comment = new Comment();
            comment.IdArticle = articleId;
            comment.Description = description;  
            comment.IdUser = userId;
            return comment;
        }
        private Blog InitializeBlog(string name, int userId, int categoryId)
        {
            
            Blog newBlog = new Blog();
            newBlog.Name = name;
            newBlog.IdUser = userId;
            newBlog.IdCategory = categoryId;
            return newBlog;
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
            using (var fs = new FileStream(env.WebRootPath + "/images/" + photo.FileName, FileMode.Create))
            {
                await photo.CopyToAsync(fs);
            }
        }

        private async Task<User> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            var currentUser = await userManager.GetUserAsync(user);
            _ = currentUser ?? throw new NotFoundException("User not found");
            return currentUser;

        }
        private async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            _ = category ?? throw new NotFoundException("Categories not found");
            return category;
        }
        private async Task CheckCategoryAsync(int id)
        {
            var category = await context.Categories.AnyAsync(x => x.Id == id);
            if (!category)
                throw new NotFoundException("CategoryException");
        }
        private async Task CheckExistedBlogAsync(int userId, int categoryId, string name)
        {
            var contains = await context.Blogs.Include(x => x.User).Include(x => x.Category)
                           .AnyAsync(x => x.IdUser == userId && x.Name == name && x.IdCategory == categoryId);
            if (contains)
                throw new ConflictException("Blog is already exists");
        }

    }
}
