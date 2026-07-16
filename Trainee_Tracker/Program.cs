using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Data;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;
using Trainee_Tracker.Data.LessonFeedbacks;
using Trainee_Tracker.Data.Rejections;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProgressService, ProgressService>();

builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<ILessonAssignmentRepository, LessonAssignmentRepository>();
builder.Services.AddScoped<ILessonDateCalculator, LessonDateCalculator>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();

builder.Services.AddScoped<LessonFeedbackService>();
builder.Services.AddScoped<ILessonFeedbackRepository, LessonFeedbackRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IRejectionRepository, RejectionRepository>();


builder.Services.AddHttpClient<WorkingHoursService>(client =>
{
    client.BaseAddress = new Uri("https://api.sopro.makandra.de/");

    var credentials = Convert.ToBase64String(
        Encoding.ASCII.GetBytes("sopro:capybara"));

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", credentials);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

var persistenceFolder = Path.Combine(Directory.GetCurrentDirectory(), "Persistence");
Directory.CreateDirectory(persistenceFolder);

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
    pattern: "{controller=Login}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
    
}

app.Run();