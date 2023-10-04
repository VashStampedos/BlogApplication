using BlogWebAPI.Services;

namespace BlogWebAPI.Configures.Services
{
    public static class ConfigureDomainServices
    {

        public static void ConfigureControllersServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<BlogService>();
            services.AddScoped<UserService>();
            services.AddScoped<EmailService>();
        }
    }
}
