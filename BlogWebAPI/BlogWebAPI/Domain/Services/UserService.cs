using AutoMapper;
using BlogWebAPI.DTO.User;
using BlogWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogWebAPI.Models;
using System.Security.Claims;
using BlogWebAPI.Domain.Exceptions;

namespace BlogWebAPI.Domain.Services
{
    public class UserService:ApplicationDbContextService
    {
        IMapper mapper;
        public UserService(
            BlogApplicationContext context,
            UserManager<User> userManager,
            IMapper mapper
            ):base(context,userManager)
        {
            this.mapper = mapper;
        }


        


        public async Task<UserResponse> GetUserAsync(int id, ClaimsPrincipal currentUser)
        {
            bool isSubscribe = false;
            var user = await context.Users.AsNoTracking().Include(x => x.Subscribes).ThenInclude(x => x.Subscriber).Include(x => x.Blogs).FirstOrDefaultAsync(x => x.Id == id);
            _ = user ?? throw new NotFoundException("User not found");

            var mappedUser = mapper.Map<UserModel>(user);
            var _currentUser = await GetCurrentUserAsync(currentUser);
           
            isSubscribe = await context.Subscribes.AsNoTracking().AnyAsync(x => x.UserId == user.Id && x.SubscriberId == _currentUser.Id);
            var response = new UserResponse { UserModel = mappedUser, IsSubscribe = isSubscribe };
            return response;
        }
        public async Task<UserModel> GetCurrentUserModelAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetCurrentUserAsync(currentUser);
            var mappedUser = mapper.Map<UserModel>(user);
            return mappedUser;

        }

        public async Task<UserResponse> SubscribeAsync(int subscribeId, ClaimsPrincipal currentUser)
        {
            var user =await GetCurrentUserAsync(currentUser);

            await CheckSubscribeAsync(subscribeId, user.Id);
            var sub = InitializeSubscribe(user.Id, subscribeId);
            await context.Subscribes.AddAsync(sub);
            await SaveChangesAsync();

            var subuser = await GetSubscriberAsync(subscribeId);

            var mappedUser = mapper.Map<UserModel>(subuser);
            return new UserResponse() { UserModel = mappedUser, IsSubscribe = true };
        }

        public async Task<UserResponse> UnSubscribeAsync(int subscribeId, ClaimsPrincipal currentUser)
        {
            var user = await GetCurrentUserAsync(currentUser);
            var subscribe = await context.Subscribes.FirstOrDefaultAsync(x => x.UserId == subscribeId && x.SubscriberId == user.Id);
            var d = "";
            _ = subscribe ?? throw new ConflictException("Subscribe not found");
            context.Subscribes.Remove(subscribe);
            await SaveChangesAsync();
            var subuser = await GetSubscriberAsync(subscribeId);

            var mappedUser = mapper.Map<UserModel>(subuser);
            return new UserResponse() { UserModel = mappedUser, IsSubscribe = false };
        }

        public async Task<IEnumerable<SubscribeModel>> GetSubscribesAsync(int userId)
        {
            var subscribes = await context.Subscribes.AsNoTracking().Include(x=> x.User).Where(x => x.SubscriberId == userId).ToListAsync();
            var sunscribesModel = mapper.Map<SubscribeModel[]>(subscribes);
            return sunscribesModel;

        }

        private async Task<User> GetSubscriberAsync(int subscribeId)
        {
            var subuser = await context.Users.AsNoTracking().Include(x => x.Subscribes).ThenInclude(x => x.Subscriber).FirstOrDefaultAsync(x => x.Id == subscribeId);
            _ = subuser ?? throw new NotFoundException("Subscriber not found");
            return subuser;
        }

        private async Task CheckSubscribeAsync(int subscribeId, int userId)
        {
            var subscribe = await context.Subscribes.AsNoTracking().AnyAsync(x => x.UserId == subscribeId && x.SubscriberId == userId);
            if (subscribe)
            {
                throw new ConflictException("Subscribe is already exist");
            }
        }

        private async Task<User> GetCurrentUserAsync(ClaimsPrincipal currentUser)
        {

            var userIdentity = await userManager.GetUserAsync(currentUser);
            var user = await context.Users.AsNoTracking().Include(x => x.Subscribes).ThenInclude(x => x.Subscriber).Include(x => x.Blogs).FirstOrDefaultAsync(x => x.Id == userIdentity.Id);
            _ = user ?? throw new NotFoundException("User not found");
            return user;
        }

        private Subscribe InitializeSubscribe(int userId, int subscriberId)
        {
            var newSub = new Subscribe();
            newSub.UserId = subscriberId;
            newSub.SubscriberId = userId;
            return newSub;
        }

    }
}
