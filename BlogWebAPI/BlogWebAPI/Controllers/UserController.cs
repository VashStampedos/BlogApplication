using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper mapper;

        public UserController(BlogApplicationContext context, UserManager<User> userManager, IMapper _mapper)
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(int id)
        {
            bool isSubscribe = false;
            var user =await _db.Users.Include(x => x.Subscribes).ThenInclude(x=> x.Subscriber).Include(x=>x.Blogs).FirstOrDefaultAsync(x=> x.Id == id);
            if (user != null)
            {
                var mappedUser = mapper.Map<UserModel>(user);
                if (User.Identity.IsAuthenticated)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var subsUser =await _db.Subscribes.FirstOrDefaultAsync(x => x.UserId == user.Id && x.SubscriberId == currentUser.Id);
                    isSubscribe = subsUser != null ? true : false;
                  
                }
                return Ok(new { UserModel = mappedUser, IsSubscribe = isSubscribe });
            }
            return BadRequest("User not found");
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var mappedUser = mapper.Map<UserModel>(user);
                    return Ok(mappedUser);
                }
            }
            return Unauthorized();
        }
        public record SubscribeRequest(int Id);
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
        {
            var subscribeId = request.Id;
            if (subscribeId > 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var subscribe = await _db.Subscribes.FirstOrDefaultAsync(x => x.UserId == subscribeId && x.SubscriberId == user.Id);
                        if (subscribe == null)
                        {
                            var newSub = new Subscribe();
                            newSub.UserId = subscribeId;
                            newSub.SubscriberId = user.Id;
                            await _db.Subscribes.AddAsync(newSub);
                            await _db.SaveChangesAsync();

                            var subuser = await _db.Users.Include(x => x.Subscribes).ThenInclude(x=> x.Subscriber).FirstOrDefaultAsync(x => x.Id == subscribeId);
                            if (subuser != null)
                            {
                                var mappedUser = mapper.Map<UserModel>(subuser);
                                return Ok(new { UserModel = mappedUser, IsSubscribe = true });
                            }

                        }
                    }
                }
            }
            
            return Unauthorized();
        }
        public record UnSubscribeRequest(int Id);
        public async Task<IActionResult> UnSubscribe([FromBody] UnSubscribeRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (request.Id > 0)
                {
                    var subscribe = await _db.Subscribes.FirstOrDefaultAsync(x => x.UserId == request.Id && x.SubscriberId == user.Id);
                    if (subscribe != null)
                    {
                        _db.Subscribes.Remove(subscribe);
                        await _db.SaveChangesAsync();
                        var subuser = await _db.Users.Include(x => x.Subscribes).ThenInclude(x => x.Subscriber).FirstOrDefaultAsync(x => x.Id == request.Id);
                        if (subuser != null)
                        {
                            var mappedUser = mapper.Map<UserModel>(subuser);
                            return Ok(new { UserModel = mappedUser, IsSubscribe = false });
                        }
                    }
                    return BadRequest();
                }
            }
            return Unauthorized(request);
        }


    }
}
