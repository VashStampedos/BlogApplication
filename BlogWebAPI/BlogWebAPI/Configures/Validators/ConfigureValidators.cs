﻿
using FluentValidation;
using System.Reflection;

namespace BlogWebAPI.Configuries.Validators
{
    public static class ConfigureValidators
    {
        public static void ConfigureValidatorServices(this IServiceCollection services) 
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddScoped<RegisterUserValidator>();
            //services.AddScoped<ConfirmEmailValidator>();
            //services.AddScoped<LoginUserValidator>();
            //services.AddScoped<CreateBlogValidator>();
            //services.AddScoped<CreateArticleValidator>();
            //services.AddScoped<CreateCommentValidator>();
            //services.AddScoped<ArticleValidator>();
            //services.AddScoped<BlogValidator>();
            //services.AddScoped<CategoryValidator>();
            //services.AddScoped<CommentValidator>();
            //services.AddScoped<LikeValidator>();
            //services.AddScoped<SubscribeValidator>();
        }
    }
}
