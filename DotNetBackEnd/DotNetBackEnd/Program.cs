using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Repositories;
using DotNetBackEnd.Infrastructure.Auth;
using DotNetBackEnd.Infrastructure.Data;
using DotNetBackEnd.Infrastructure.Messaging;
using DotNetBackEnd.Infrastructure.Messaging.Consumers;
using DotNetBackEnd.Infrastructure.Repositories.Ef;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Kafka config
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));

// EF Core + SQL Server
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

builder.Services.AddDbContext<AuthDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));


builder.Services
    .AddIdentity<AppUser, IdentityRole>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireDigit = false;
        opt.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

var jwt = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromSeconds(10)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
});

// Repository
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

// Event bus
builder.Services.AddSingleton<IEventBus, KafkaEventBus>();

// Kafka background consumers
builder.Services.AddHostedService<PaymentProcessorService>();
builder.Services.AddHostedService<InventoryProcessorService>();
builder.Services.AddHostedService<OrderCompletionProcessorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/health", () => "OK");

await SeedAuthAsync(app);

static async Task SeedAuthAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    foreach (var role in new[] { "admin", "user" })
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new IdentityRole(role));

    async Task ensure(string username, string password, string role)
    {
        var u = await userMgr.FindByNameAsync(username);
        if (u == null)
        {
            u = new AppUser { UserName = username, Email = $"{username}@local" };
            await userMgr.CreateAsync(u, password);
            await userMgr.AddToRoleAsync(u, role);
        }
    }

    await ensure("admin", "admin123", "admin");
    await ensure("user", "user123", "user");
}

app.Run();
