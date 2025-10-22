using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PD.BL.Services.UserService;
using PD.DAL;
using PD.DAL.Interface;
using PD.DAL.Repository;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Dtos.UserDtos;
using PD.BL.Validators.UserValidator;
using PD.BL.Helpers;




var builder = WebApplication.CreateBuilder(args);


//Servis ve repisotory tanitimi
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(UserService).Assembly);
});

builder.Services.AddControllers();
//fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<UserService>();

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
