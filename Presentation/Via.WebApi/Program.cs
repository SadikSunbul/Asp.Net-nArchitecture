using System.Threading.RateLimiting;
using Via.Application;
using Via.Infrastructer;
using Via.Persistence;
using Via.WebApi.Extensions;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructerServices();
builder.Services.AddPersistenceServices(builder.Configuration);

//Cacheleme Servisleri baðlantýsý
// builder.Services.AddCache(Via.Domain.Enums.CacheType.Redis, builder.Configuration);

//Log ýslemýnde kulalndýgýmýz ýcýn buraya ekledýk bunu 
builder.Services.AddHttpContextAccessor(); 


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();






var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//global hata yonetimi
if (app.Environment.IsProduction())
{
    app.UseGlobalExceptionMidleware();
}
app.UseGlobalExceptionMidleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
