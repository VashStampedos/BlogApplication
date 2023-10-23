using AutoMapper;
using BlogWebAPI.Domain.Services;
using BlogWebAPI.DTO.Auth;
using BlogWebAPI.Entities;
using BlogWebAPI.Results;
using BlogWebAPI.Storages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace BlogWebAPI.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
       
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        AccountService accountService;
        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            AccountService accountService,
            ValidatorsStorage validators
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.accountService = accountService;

        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
           
            if(ModelState.IsValid)
            {
                await accountService.CreateUserAsync(request);
                return Ok(ApiResult<string>.Success("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме"));

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));

            
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
           
            if (ModelState.IsValid)
            {
                await accountService.ConfirmUserEmailAsync(request.UserId, request.Token);
                return Redirect(request.ReturnUrl);
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }

      
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogInRequest request)
        {
           
            if (ModelState.IsValid)
            {
                var logInResult =await accountService.LogInAsync(request.Email, request.Password );
                if(logInResult)
                    return Ok(ApiResult<object>.Success(new { isSuccess = true, message = "Success login" })) ;
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));
        }
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return Ok(ApiResult<string>.Success("succeesed logout"));
                
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Unauthorized" }));
        }
        

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserClaims()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user =await _userManager.GetUserAsync(User);
                if (await _userManager.IsEmailConfirmedAsync(user!))
                {
                    var userClaims = User.Claims.Select(x => new { type = x.Type, value = x.Value }).ToList();
                    return Ok(ApiResult<object>.Success(userClaims)) ;

                }
            }
            return Unauthorized(ApiResult<string>.Failure(HttpStatusCode.Unauthorized, new List<string>() { "Unauthorized" })) ;
        }
       
    }


}
