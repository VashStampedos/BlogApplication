using BlogWebAPI.Domain.Exceptions;
using BlogWebAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogWebAPI.Domain.Services
{
    public class ApplicationDbContextService
    {
        public readonly BlogApplicationContext context;
        public readonly UserManager<User> userManager;

        public ApplicationDbContextService(BlogApplicationContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task SaveChangesAsync()
        {
            int result;
            try
            {
                result = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ConflictException("Can not save changes to database", ex);
            }
            if(result < 1)
            {
                throw new ConflictException("Changes don't saved");
            }
        }
    }
}
