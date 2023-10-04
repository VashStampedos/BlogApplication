using AutoMapper;
using BlogWebAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogWebAPI.Services
{
    public class BlogService
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        EmailService _emailService;
        IMapper mapper;

        public BlogService(
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
    }
}
