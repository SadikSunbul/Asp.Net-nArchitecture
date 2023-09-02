using Via.Application;
using Via.Infrastructer;
using Via.Persistence;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructerServices();
builder.Services.AddPersistenceServices(builder.Configuration);


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Global hata yönetimi 
app.UseGlobalExceptionMidleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
