using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using WebApp.Configrations;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.RegisterDbContexts(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);


builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.SignIn.RequireConfirmedAccount = false;
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;

}).AddEntityFrameworkStores<WebAppContext>();
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin";
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
});

builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "";
    x.AppSecret = "";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");
});
builder.Services.AddAuthentication().AddGoogle(x =>
{
    x.ClientId = "";
    x.ClientSecret = "";
});

var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePagesWithRedirects("/StatusCodeError/{0}");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
