// Program.cs
using AnimArt.Data;
using AnimArt.Entities;
using AnimArt.Interfaces;
using AnimArt.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Додайте сервіси автентифікації
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "CookieAuth";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Реєстрація DataStorage
builder.Services.AddSingleton<IDataStorage<User>, JsonStorage<User>>();
builder.Services.AddSingleton<IDataStorage<Anime>, JsonStorage<Anime>>();
builder.Services.AddSingleton<IDataStorage<Review>, JsonStorage<Review>>();
builder.Services.AddSingleton<IDataStorage<Rating>, JsonStorage<Rating>>();
builder.Services.AddSingleton<IDataStorage<UserLists>, JsonStorage<UserLists>>();

// Реєстрація репозиторіїв з використанням інтерфейсів
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IRepository<Review>, Repository<Review>>();
builder.Services.AddScoped<IRepository<Rating>, Repository<Rating>>();
builder.Services.AddScoped<IRepository<UserLists>, Repository<UserLists>>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();