using AutoMapper;
using BlogWebAPI.DTO.Auth;
using BlogWebAPI.Entities;
using BlogWebAPI.Results;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;

namespace BlogWebAPI.Services
{
    public class AccountService
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        EmailService _emailService;
        IMapper mapper;

        public AccountService(
            BlogApplicationContext context, 
            IMapper _mapper, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            EmailService emailService)
        {
            _db = context;
            _userManager = userManager; 
            _signInManager = signInManager;
            _emailService = emailService;    
            mapper = _mapper;
        }

        private async Task<bool> CheckUserbyEmailAsync(string email)
        {
            var normalizedEmail = _userManager.NormalizeEmail(email);
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            return user != null ? true:false;
        }
        

        private async Task<User> GetUserbyEmailAsync(string email)
        {
            var normalizedEmail = _userManager.NormalizeEmail(email);
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            return user;
        }
        private User InitializeUser(string username, string email)
        {
            return new User
            {
                UserName = username,
                Email = email
            };

        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            var cuurentUser = await _userManager.GetUserAsync(user);
            return cuurentUser;
        }

        public async Task<RegisterResult> CreateUserAsync(RegisterUserRequest requset)
        {
            string username = requset.UserName;
            string email = requset.Email;
            string password = requset.Password;
            
                
            if (!await CheckUserbyEmailAsync(email))
            {
                var user = InitializeUser(username, email);
            
                var createResult = await _userManager.CreateAsync(user, password);
            
                if (createResult.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return RegisterResult.Success(token, user);
                    
                }
            }
            return RegisterResult.Failure();
        }

        public async Task SendConfirmMessageToUserEmailAsync(string email, string callback)
        {
            await _emailService.SendEmailAsync(email, "Подтверждение", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callback}'> Подтвердить!!!!</a>");
        }

        public async Task<bool> ConfirmUserEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            var user = await GetUserbyEmailAsync(email);
            if ( user != null)
            {
               var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
               return result.Succeeded;
            }
            return false;
        }

      



    }
}
