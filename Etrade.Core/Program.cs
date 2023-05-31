
using Etrade.DAL.Abstract;
using Etrade.DAL.Concrete;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EtradeDbContext>();
builder.Services.AddScoped<ICategoryDAL,CategoryDAL>();
builder.Services.AddScoped<IProductDAL,ProductDAL>();
builder.Services.AddScoped<IOrderDAL,OrderDAL>();
builder.Services.AddScoped<IOrderlineDAL,OrderlineDAL>();


//AddIdentity
builder.Services.AddIdentity<User, Role>(options =>
{
    //lockout->kilitlenme
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    //password
    options.Password.RequiredUniqueChars=0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<EtradeDbContext>().AddDefaultTokenProviders(); 

//Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath="/Account/SignIn";//Giriþ yapýlmadýysa
    options.AccessDeniedPath="/";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.Cookie = new CookieBuilder
    {
        HttpOnly=false,
        SameSite=SameSiteMode.Lax,
        SecurePolicy=CookieSecurePolicy.Always
    };
});
//AddSession
builder.Services.AddSession();
builder.Services.AddAuthentication().AddGoogle(options =>
{
    /*options.ClientId = System.Configuration.ConfigurationManager.AppSettings["Authentication:Google:ClientId"];
    options.ClientSecret = System.Configuration.ConfigurationManager.AppSettings["Authentication:Google:ClientSecret"];*/

    options.ClientId = "816421377277-fc9sbh58407grinna53n68vjt6ngg2h7.apps.googleusercontent.com";
    options.ClientSecret = "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";

});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//UseSession
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Identity iþlemi
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
