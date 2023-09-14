using Microsoft.AspNetCore.Identity;

namespace BlogWebAPI.Entities
{
    public class User: IdentityUser<int>
    {


        //public int Id { get; set; }
        //public string Email { get; set; } = null!;
        //public string Password { get; set; } = null!;
        public virtual ICollection<Blog> Blogs { get; set; } = null!;
        public virtual ICollection<Like> Likes { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
        public virtual ICollection<Subscribe> Subscribes { get; set; } = null!;
    }

    public class Role : IdentityRole<int>
    {

    }
    }
