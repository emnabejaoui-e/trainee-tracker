using TutorialProject.Data;
using TutorialProject.Data.Lessons;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var lessonContext = new LessonContext(false);
lessonContext.Database.EnsureCreated();
DbInitializer.InitializeDatabase(lessonContext);

var lessonRepository = new DatabaseLessonRepository(lessonContext);

builder.Services.AddSingleton<ILessonRepository>(lessonRepository);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
