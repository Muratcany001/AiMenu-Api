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
using PD.BL.Services.AuthService;
using PD.BL.Helpers.JwtHelper;
using PD.BL.Services.MenuItemService;
using PD.BL.Services.OrderItemService;
using Dtos.MenuItemDto;
using PD.BL.Validators.MenuItemValidator;
using Dtos.OrderItemDto;
using PD.BL.Validators.OrderItemValidator;
using PD.BL.Services.OrderService;
using PD.BL.Helpers.OrderHelper;




var builder = WebApplication.CreateBuilder(args);


//services
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//validators
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<AddMenuItemDto>, AddMenuItemDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateMenuItemDto>, UpdateMenuItemDtoValidator>();
builder.Services.AddScoped<IValidator<MenuItemDto>, MenuItemDtoValidator>();
builder.Services.AddScoped<IValidator<CreateOrderItemDto>, CreateOrderItemDtoValidator>();
builder.Services.AddScoped<IValidator<SetQuantityDto>, SetQuantityValidator>();
builder.Services.AddScoped<IValidator<UpdateOrderItemNoteDto>, UpdateOrderItemNoteDtoValidator>();
//helpers
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<HashHelper, HashHelper>();
builder.Services.AddScoped<OrderNumberHelper, OrderNumberHelper>();


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
