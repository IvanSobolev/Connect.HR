using System.Text;
using AuthMicroservice.Managers.Implementations;
using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models;
using AuthMicroservice.Repositories.Implementations;
using AuthMicroservice.Repositories.Interfaces;
using AuthMicroservice.Services.Implementation;
using AuthMicroservice.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggingBuilder => 
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var connectionString = builder.Configuration.GetValue<string>("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set");

builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IProfileRepository, EfCoreProfileRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, EfCoreRefreshTokenRepository>();

builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
builder.Services.AddScoped<IProfileMicroserviceClient, ProfileMicroserviceClient>();

builder.Services.AddScoped<IProfileManager, ProfileManager>();
builder.Services.AddScoped<IRefreshTokenManager, RefreshTokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AuthOptions:issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AuthOptions:audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:key"] ?? throw new InvalidOperationException("AuthOptions:key is not configured"))),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Microservice API", Version = "v1" });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();


app.Run();
