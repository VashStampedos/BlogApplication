using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
{
    options.UseSqlServer(connection, b=> b.MigrationsAssembly(assembly));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AspNetIdentityDbContext>();
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options => 
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connection, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connection, opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();



var app = builder.Build();

app.UseIdentityServer();

app.Run();
