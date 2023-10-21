using AutoMapper;
using BlogWebAPI.DTO.User;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using BlogWebAPI.Results;
using BlogWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        
        UserService userService;

        public UserController(
            UserService userService
            )
        {
            this.userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(int id)
        {
            if(id >0)
            {  
                var result =await userService.GetUserAsync(id, User);
                return Ok(ApiResult<UserResponse>.Success(result));
                
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() {"Invalid requset"}));
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userModel = await userService.GetCurrentUserModelAsync(User);
                return Ok(ApiResult<UserModel>.Success(userModel));
            }
            return Unauthorized(ApiResult<string>.Failure(HttpStatusCode.Unauthorized, new List<string>() { "Unauthorized" }));
        }

        public async Task<IActionResult> GetUserSubscribes(int id)
        {
            if(id>0)
            {
                var subscribesModel = await userService.GetSubscribesAsync(id);
                return Ok(ApiResult<IEnumerable<SubscribeModel>>.Success(subscribesModel));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid requset" }));
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
        {
            
            var subscribeId = request.Id;
            if (subscribeId > 0)
            {

                var response = await userService.SubscribeAsync(subscribeId, User);
                return Ok(ApiResult<UserResponse>.Success(response));
            }
                        
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid requset" }));
        }
        public async Task<IActionResult> UnSubscribe([FromBody] SubscribeRequest request)
        {
            var subscribeId = request.Id;
            if (subscribeId > 0)
            {
                var response =await userService.UnSubscribeAsync(subscribeId, User); 
                return Ok(ApiResult<UserResponse>.Success(response));
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid requset" }));
        }


    }
}
