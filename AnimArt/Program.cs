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
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Куки на 7 днів
    });

// Реєстрація DataStorage
builder.Services.AddSingleton<IDataStorage<User>, JsonStorage<User>>();
builder.Services.AddSingleton<IDataStorage<Anime>, JsonStorage<Anime>>();
builder.Services.AddSingleton<IDataStorage<Genre>, JsonStorage<Genre>>();
builder.Services.AddSingleton<IDataStorage<Studio>, JsonStorage<Studio>>();
builder.Services.AddSingleton<IDataStorage<VoiceStudio>, JsonStorage<VoiceStudio>>();
builder.Services.AddSingleton<IDataStorage<Review>, JsonStorage<Review>>();
builder.Services.AddSingleton<IDataStorage<Rating>, JsonStorage<Rating>>();
builder.Services.AddSingleton<IDataStorage<UserLists>, JsonStorage<UserLists>>();

// Реєстрація репозиторіїв
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IRepository<Genre>, Repository<Genre>>();
builder.Services.AddScoped<IRepository<Studio>, Repository<Studio>>();
builder.Services.AddScoped<IRepository<VoiceStudio>, Repository<VoiceStudio>>();
builder.Services.AddScoped<IRepository<Review>, Repository<Review>>();
builder.Services.AddScoped<IRepository<Rating>, Repository<Rating>>();
builder.Services.AddScoped<IRepository<UserLists>, Repository<UserLists>>();

// Реєстрація сервісів
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

// Важливо: UseAuthentication має бути перед UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();