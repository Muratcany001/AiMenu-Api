using Docker.DotNet.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PD.BL.Helpers;
using PD.BL.Helpers.GeminiHelper;
using PD.BL.Helpers.JwtHelper;
using PD.BL.Helpers.OrderHelper;
using PD.BL.Helpers.TelegramHelper;
using PD.BL.Services.AuthService;
using PD.BL.Services.MenuItemService;
using PD.BL.Services.OrderItemService;
using PD.BL.Services.OrderService;
using PD.BL.Services.RedisCacheService;
using PD.BL.Services.UserService;
using PD.DAL;
using PD.DAL.Interface;
using PD.DAL.Repository;
using Serilog;
using System.Text;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// ============ SERVICES ============
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddHttpClient<IMenuItemService, MenuItemService>();

// ============ REPOSITORIES ============
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// ============ HELPERS ============
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<HashHelper, HashHelper>();
builder.Services.AddScoped<OrderNumberHelper, OrderNumberHelper>();
builder.Services.AddHttpClient<GeminiHelper>();
builder.Services.AddScoped<ITelegramHelper,TelegramHelper>();
builder.Services.AddMemoryCache();

// ============ FLUENT VALIDATION ============
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserService>();

// ============ AUTOMAPPER ============
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserService).Assembly);
});

// ============ AUTHORIZATION ============
builder.Services.AddAuthorization();

// ============ CONTROLLERS ============
builder.Services.AddControllers();

// ============ SWAGGER ============
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ============ CORS ============
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// ============ SECRETS ============
builder.Configuration.AddUserSecrets<Program>();
DotNetEnv.Env.Load();

// ============ DATABASE ============
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============ REDIS ============
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//============ SERILOG ============
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

//============ BUILDER ============
var app = builder.Build();

// ============ MIDDLEWARE PIPELINE ============
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//  CORS mutlaka burada ve policy adıyla kullanılacak
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//============ SERILOG BENCHMARK ============
app.UseSerilogRequestLogging();
app.Run();
