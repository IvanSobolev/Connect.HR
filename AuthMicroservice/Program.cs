using System.Text;
using AuthMicroservice.Managers.Implementations;
using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Repositories.Implementations;
using AuthMicroservice.Repositories.Interfaces;
using AuthMicroservice.Services.Implementation;
using AuthMicroservice.Services.Interfaces;
using Grpc.Net.Client;
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

AuthOptions authOptions = new AuthOptions(Environment.GetEnvironmentVariable("AUTH_OPTIONS") ??
                                          throw new InvalidOperationException(
                                              "AuthOptions: authOptions is not configured"));
GrpcOptions grpcOptions = new GrpcOptions(Environment.GetEnvironmentVariable("GRPC_ADDRESS") ??
                                          throw new InvalidOperationException(
                                              "GrpcOptions: grpcOptions is not configured"));

builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));

builder.Services.AddSingleton(authOptions);
builder.Services.AddSingleton(grpcOptions);

builder.Services.AddScoped<IProfileRepository, EfCoreProfileRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, EfCoreRefreshTokenRepository>();

builder.Services.AddScoped<IProfileMicroserviceClient, ProfileMicroserviceClient>();
builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

builder.Services.AddScoped<IProfileManager, ProfileManager>();
builder.Services.AddScoped<IRefreshTokenManager, RefreshTokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authOptions.Key)),
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
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.Run();
