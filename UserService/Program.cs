using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Common;
using UserService.DataAccess;
using UserService.Extensions;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authSettings = builder.Configuration.GetSection(AuthSettingOptions.AuthOption).Get<AuthSettingOptions>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(authSettings.Connection));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
