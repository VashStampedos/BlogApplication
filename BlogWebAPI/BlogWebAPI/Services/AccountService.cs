using AutoMapper;
using BlogWebAPI.DTO.Auth;
using BlogWebAPI.Entities;
using BlogWebAPI.Exceptions;
using BlogWebAPI.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit.Encodings;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Policy;
using System.Web;

namespace BlogWebAPI.Services
{
    public class AccountService:ApplicationDbContextService
    {
        SignInManager<User> signInManager;
        EmailService emailService;

        public AccountService(
            BlogApplicationContext context, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            EmailService emailService):base(context, userManager)
        {
           
            this.signInManager = signInManager;
            this.emailService = emailService;    
        }


        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            
            var currentUser = await userManager.GetUserAsync(user);
            _ = currentUser ?? throw new NotFoundException("User not found");
            return currentUser;
        }

        public async Task CreateUserAsync(RegisterUserRequest requset)
        {
            string username = requset.UserName;
            string email = requset.Email;
            string password = requset.Password;
            string returnUrl = requset.ReturnUrl;

            await CheckUserByEmailAsync(email);

            var user = InitializeUser(username, email);
            
            var createResult = await userManager.CreateAsync(user, password);
            
            if (createResult.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var message = GenerateConfirmMessage(token, user.Id, returnUrl);
                await SendConfirmMessageToUserEmailAsync(email, message);


            }
        }


        public async Task ConfirmUserEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            _ = user ?? throw new NotFoundException("User not found");
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new ConflictException("Can't confirm email");
        }
        private string GenerateConfirmMessage(string token, int userId, string returnUrl)
        {
            var codedToken = HttpUtility.UrlEncode(token);   
            var urlBuilder = new UriBuilder();
            urlBuilder.Scheme = "https";
            urlBuilder.Host = "localhost";
            urlBuilder.Port = 7018;
            urlBuilder.Path = $"/Account/ConfirmEmail";
            urlBuilder.Query = $"userId={userId}&returnUrl={returnUrl}&token={codedToken}";
            return urlBuilder.ToString();
        }

        private async Task SendConfirmMessageToUserEmailAsync(string email, string callback)
        {
            await emailService.SendEmailAsync(email, "Подтверждение", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callback}'> Подтвердить!!!!</a>");
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            var user = await GetUserbyEmailAsync(email);
            if ( user != null)
            {
               var result = await signInManager.PasswordSignInAsync(user, password, true, false);
               return result.Succeeded;
            }
            return false;
        }
        private async Task CheckUserByEmailAsync(string email)
        {
            var normalizedEmail = userManager.NormalizeEmail(email);
            var user = await userManager.FindByEmailAsync(normalizedEmail);
            if(user != null)
            {
                throw new ConflictException("User is already exists");
            }
          
        }
        

        private async Task<User> GetUserbyEmailAsync(string email)
        {
            var normalizedEmail = userManager.NormalizeEmail(email);
            var user = await userManager.FindByEmailAsync(normalizedEmail);
            _ = user ?? throw new NotFoundException($"User with {email} not found");
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
    }
}
