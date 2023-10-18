using AutoMapper;
using BlogWebAPI.DTO.User;
using BlogWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogWebAPI.Models;
using System.Security.Claims;
using BlogWebAPI.Results;
using System.Net;

namespace BlogWebAPI.Services
{
    public class UserService
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper _mapper;
        public UserService(
            BlogApplicationContext context,
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _db = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResult<UserResponse>> GetUserAsync(int id, ClaimsPrincipal currentUser)
        {
            bool isSubscribe = false;
            var user = await _db.Users.Include(x => x.Subscribes).ThenInclude(x => x.Subscriber).Include(x => x.Blogs).FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                var mappedUser = _mapper.Map<UserModel>(user);
                var _currentUser = await _userManager.GetUserAsync(currentUser);
                var subsUser = await _db.Subscribes.FirstOrDefaultAsync(x => x.UserId == user.Id && x.SubscriberId == _currentUser.Id);
                isSubscribe = subsUser != null ? true : false;
                var response = new UserResponse { UserModel = mappedUser, IsSubscribe = isSubscribe };
                return ApiResult<UserResponse>.Success(response);
              
            }
            return ApiResult<UserResponse>.Failure(HttpStatusCode.NotFound, new List<string>() { "User not found" });
        }

    }
}
