using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogWebAPI.Entities
{
    public class BlogApplicationContext: IdentityDbContext<User, Role, int>
    {
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
       // public DbSet<User> Users { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Subscribe> Subscribes { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public BlogApplicationContext(DbContextOptions<BlogApplicationContext> options):base(options)
        {
            
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
               
                //    //entity.Property(x => x.Email).IsRequired();
                //    //entity.Property(x => x.Password).IsRequired();

                entity.HasData(
                    new User { Id = 1, Email = "jin-gennadevich@mail.ru", PasswordHash = "AQAAAAIAAYagAAAAEEI03B8VDY91eT1smNi+Kdo+oeOmZeS3SHYisTX3aUw4/kgaRWtBAzL+SHIWzRI+Gg==" },
                    new User { Id = 2, Email = "sema-chuma@mail.ru", PasswordHash = "AQAAAAIAAYagAAAAEEI03B8VDY91eT1smNi+Kdo+oeOmZeS3SHYisTX3aUw4/kgaRWtBAzL+SHIWzRI+Gg==" },
                    new User { Id = 3, Email = "max-shket@mail.ru", PasswordHash = "AQAAAAIAAYagAAAAEEI03B8VDY91eT1smNi+Kdo+oeOmZeS3SHYisTX3aUw4/kgaRWtBAzL+SHIWzRI+Gg==" }


                    ) ;
            });

            modelBuilder.Entity<Blog>(entity => 
            {
                entity.ToTable("Blogs");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.IdUser).IsRequired();
                entity.HasOne(x => x.User).WithMany(u => u.Blogs).HasForeignKey(x => x.IdUser).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Category).WithMany(c => c.Blogs).HasForeignKey(x => x.IdCategory).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x=> x.Articles).WithOne(a => a.Blog).HasForeignKey(x => x.IdBlog).OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                   new Blog { Id = 1, Name = "Races", IdUser = 1, IdCategory = 1},
                   new Blog { Id = 2, Name = "Races", IdUser = 2, IdCategory = 2 },
                   new Blog { Id = 3, Name = "Cooking", IdUser = 1, IdCategory = 2 },
                   new Blog { Id = 4, Name = "Learning", IdUser = 1, IdCategory = 1 },
                   new Blog { Id = 5, Name = "Gaming", IdUser = 3, IdCategory = 1 },
                   new Blog { Id = 6, Name = "Walking", IdUser = 1, IdCategory = 1 }


                   );
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired();
                entity.HasData(
                    new Category { Id = 1, Name = "Gaiety" },
                    new Category { Id = 2, Name = "Science" },
                    new Category { Id = 3, Name = "Nature" },
                    new Category { Id = 4, Name = "Animals" }
                    );
            });
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Articles");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Title).IsRequired();
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.Photo);
                entity.Property(x => x.IdBlog).IsRequired();
                entity.HasOne(x => x.Blog).WithMany(u => u.Articles).HasForeignKey(x => x.IdBlog).OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                   new Article { Id = 1, Title = "New Racer", Description = "lijdehfklhdfklghuds", Photo = "None" ,IdBlog = 1 },
                   new Article { Id = 2, Title = "New Knife", Description = "dsacxs", Photo = "None", IdBlog = 2 },
                   new Article { Id = 3, Title = "New Sport", Description = "asdttqwr", Photo = "None", IdBlog = 3 },
                   new Article { Id = 4, Title = "New Racer 2", Description = "321aasd", Photo = "None", IdBlog = 1 },
                   new Article { Id = 5, Title = "New lesson", Description = "asdra4213qwdasd", Photo = "None", IdBlog = 4 }

                   );
            });
            //noaction
            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("Likes");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.Id);
                entity.Property(x => x.IdUser);
                entity.Property(x => x.IdArticle).IsRequired();
                entity.HasOne(x => x.User).WithMany(u => u.Likes).HasForeignKey(x => x.IdUser).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(x => x.Article).WithMany(a => a.Likes).HasForeignKey(x => x.IdArticle).OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                   new Like { Id = 1, IdUser = 1, IdArticle = 1},
                   new Like { Id = 2, IdUser = 1, IdArticle = 2},
                   new Like { Id = 3, IdUser = 2, IdArticle = 1},
                   new Like { Id = 4, IdUser = 2, IdArticle = 2},
                   new Like { Id = 5, IdUser = 3, IdArticle = 3}

                   );
            });
            //noaction
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.IdUser).IsRequired();
                entity.Property(x => x.IdArticle).IsRequired();
                entity.HasOne(x => x.User).WithMany(u => u.Comments).HasForeignKey(x => x.IdUser).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(x => x.Article).WithMany(a => a.Comments).HasForeignKey(x => x.IdArticle).OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                   new Comment { Id = 1, Description="1/1", IdUser = 1, IdArticle = 1 },
                   new Comment { Id = 2, Description = "1/3", IdUser = 1, IdArticle = 3 },
                   new Comment { Id = 3, Description = "2/1", IdUser = 2, IdArticle = 1 },
                   new Comment { Id = 4, Description = "2/3", IdUser = 2, IdArticle = 3 }
                   );
            });
            modelBuilder.Entity<Subscribe>(entity =>
            {
                entity.ToTable("Subscribes");
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.SubscriberId).IsRequired();
                entity.HasKey(e => new { e.UserId, e.SubscriberId });
                entity.HasOne(x => x.User).WithMany(u => u.Subscribes).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasData(
                   new Subscribe { UserId = 1, SubscriberId = 2 },
                   new Subscribe { UserId = 1, SubscriberId = 3 },
                   new Subscribe { UserId = 2, SubscriberId = 3 },
                   new Subscribe { UserId = 2, SubscriberId = 1 },
                   new Subscribe { UserId = 3, SubscriberId = 1 }
                   );
            });

        }


    }
}
