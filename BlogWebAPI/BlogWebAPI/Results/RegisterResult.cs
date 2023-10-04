using BlogWebAPI.Entities;
using System.Net;

namespace BlogWebAPI.Results
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public User User{ get; set; }

        public RegisterResult()
        {
            
        }

        public RegisterResult(bool succeeded, string token, User user)
        {
            Succeeded = succeeded;
            Token = token;
            User = user;
        }

        public static RegisterResult Success(string token, User user) => new RegisterResult(true, token, user);
        public static RegisterResult Failure() => new RegisterResult(false, default, default);
    }
}
