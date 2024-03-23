using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Setup app data
SetupAddData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

static void SetupAddData(WebApplication app)
{
    using var serviceScope = ((IApplicationBuilder)app)
        .ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

    context!.Database.Migrate();

    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
    var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();

    var adminRole = new AppRole { Name = "Admin" };
    var userRole = new AppRole { Name = "User" };

    var res = roleManager!.CreateAsync(adminRole).Result;

    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }

    var adminUser = new AppUser
    {
        Email = "admin@pats.ee",
        UserName = "admin@pats.ee"
    };

    res = userManager!.CreateAsync(adminUser, "Foo.bar1").Result;

    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }

    res = userManager!.AddToRoleAsync(adminUser, "Admin").Result;

    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }
}