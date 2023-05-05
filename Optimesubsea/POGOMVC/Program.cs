using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POGOMVC.DataLayer;
using POGOMVC.Models;
using POGOMVC.Views.Projects;

var builder = WebApplication.CreateBuilder(args);
BaseSessionModel.SecrateKey = builder.Configuration.GetValue<string>("EncryptDecryptKey");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<GasStationProjectDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<GasStationProjectDbContext>();
builder.Services.AddScoped<ProjectsController>();
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);//Need to configure in App.json file
    //options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
});


var app = builder.Build();

var v1 = builder.Configuration.GetValue<bool>("SeedData");
//if (args.Length == 1 && args[0].ToLower() == "seeddata")
if (v1)
{
    SeedData(app);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserRegistration}/{action=Login}");
//pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    if (scopedFactory != null)
    {
        using (var scope = scopedFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<DataSeeder>();
            if (service != null)
            {
                service.Seed();
            }
        }
    }
}