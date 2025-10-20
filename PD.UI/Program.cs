using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PD.BL.Services.UserService;
using PD.DAL;
using PD.DAL.Interface;
using PD.DAL.Repository;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


//Servis ve repisotory tanitimi
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
var mappingAssemblies = new[] { typeof(PD.BL.Services.UserService.UserService).Assembly };
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PD.BL.Services.UserService.UserService>();


builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(mappingAssemblies);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
