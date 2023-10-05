using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using BlogWebAPI.Services;
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
        UserService _userService;

        public UserController(BlogApplicationContext context, 
            UserManager<User> userManager, 
            IMapper _mapper,
            UserService userService
            )
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(int id)
        {
            if(id >0)
            {
                if (User.Identity.IsAuthenticated)
                {
                     
                    var result =await _userService.GetUserAsync(id, User);
                    if(result.Succeeded)
                    return Json(result.Data);
                

                }

            }
            return BadRequest();
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
