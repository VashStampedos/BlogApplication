using AutoMapper;
using BlogWebAPI.DTO.Auth;
using BlogWebAPI.Entities;
using BlogWebAPI.Results;
using BlogWebAPI.Services;
using BlogWebAPI.Storages;
using BlogWebAPI.Validators.RequestsValidators.AuthValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogWebAPI.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
       
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        ValidatorsStorage _validators;
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
            _validators = validators;

        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var validationResult = await _validators._registerValidator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
               await accountService.CreateUserAsync(request);


                //var callback = Url.Action(
                //    "ConfirmEmail",
                //    "Account",
                //    new { userid = registerResult.User.Id, token = registerResult.Token },
                //    protocol: HttpContext.Request.Scheme);

                //await _accountService.SendConfirmMessageToUserEmailAsync(registerResult.User.Email, callback);
                return Json("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
               
            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));

            
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            var validationResult = await _validators._confirmEmailValidator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var confirmResult =await accountService.ConfirmUserEmailAsync(request.UserId, request.Token);
                if (confirmResult)
                {
                    //
                    return RedirectToAction("Blogs", "Blog");

                }
            }
            return BadRequest();
        }

      
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogInRequest request)
        {
            var result =await _validators._logInValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var logInResult =await accountService.LogInAsync(request.Email, request.Password );
                if(logInResult)
                    return Ok(new { isSuccess = true, message = "Success login" }) ;
            }
            return Unauthorized();
        }
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return Ok();
                
            }
            return BadRequest();
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
                    return Ok(userClaims);

                }
            }
            return Unauthorized();
        }
       
    }


}
