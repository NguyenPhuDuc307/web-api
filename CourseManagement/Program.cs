using Microsoft.EntityFrameworkCore;
using CourseManagement.Data;
using CourseManagement.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDbContext") ?? throw new InvalidOperationException("Connection string 'CourseDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//DI configuration
builder.Services.AddTransient<ICoursesService, CoursesService>();
builder.Services.AddTransient<ILessonsService, LessonsService>();

//File Storage
builder.Services.AddTransient<IStorageService, FileStorageService>();

//automapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Course Management", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Course Management V1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DbInitializer.Seed(services);
}

app.Run();
