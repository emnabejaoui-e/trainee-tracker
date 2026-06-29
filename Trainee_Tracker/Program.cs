// Code Owner: Jelena Cosic
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Data;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Code-Owner: Andrej Basara
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILessonAssignmentRepository, StaticLessonAssignemtRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.EnsureCreated();

    if (!db.Trainees.Any() && !db.Mentors.Any() && !db.Admins.Any())
    {
        db.Trainees.Add(new Trainee { Name = "Jelena3 Trainee", Email = "jelenacosic3@makandra.de", HashedPassword = BCrypt.Net.BCrypt.HashPassword("12345"), Closed = false });
        db.Mentors.Add(new Mentor   { Name = "Jelena2 Mentor",  Email = "jelenacosic2@makandra.de", HashedPassword = BCrypt.Net.BCrypt.HashPassword("12345"), Closed = false, Curriculum = null! });
        db.Admins.Add(new Admin     { Name = "Jelena3 Admin",   Email = "jelenacosic1@makandra.de", HashedPassword = BCrypt.Net.BCrypt.HashPassword("12345"), Closed = false });
        db.SaveChanges();
    }
}

app.Run();