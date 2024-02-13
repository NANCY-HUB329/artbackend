using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Arts.Data;
using Arts.Models;
using Arts.Models.Dtos;
using Arts.Services.IService;
using Arts.Services;
using Arts;
using Microsoft.OpenApi.Models;
using Arts.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

///

// Add your DbContext and other services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mydbconnection"));
});

builder.Services.AddScoped<IArtService, ArtService>();

// Configure AutoMapper profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// This is where you add your AutoMapper configuration
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<ArtDto, Art>();
    cfg.CreateMap<Art, ArtDto>();
});

// Register the IMapper instance
builder.Services.AddSingleton<IMapper>(new Mapper(mapperConfig));

// ===============================ALLOW CORS=======================================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});
//from extensions
builder.AddSwaggenGenExtension();
builder.AddAuth();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AUTH API");
        c.RoutePrefix = string.Empty;
    }
});

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});
app.UseHttpsRedirection();
app.UseMigrations();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(); // Enable CORS middleware
app.MapControllers();
app.Run();


//

