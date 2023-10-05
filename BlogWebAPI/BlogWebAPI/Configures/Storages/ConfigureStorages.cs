using BlogWebAPI.Storages;

namespace BlogWebAPI.Configures.Storages
{
    public static class ConfigureStorages
    {
        public static void ConfigureValidatorsStorage(this IServiceCollection services)
        {
            services.AddScoped<ValidatorsStorage>();
        }
    }
}
