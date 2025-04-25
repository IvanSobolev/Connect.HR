using System.Text;
using AuthMicroservice.Models.Dtos;
using MatchMicroservice.Manager.Implementations;
using MatchMicroservice.Manager.Interfaces;
using MatchMicroservice.Models;
using MatchMicroservice.Repositories.Implementations;
using MatchMicroservice.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AuthOptions authOptions = new AuthOptions(Environment.GetEnvironmentVariable("AUTH_OPTIONS") ??
                                          throw new InvalidOperationException(
                                              "AuthOptions: authOptions is not configured"));

builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));

builder.Services.AddScoped<ISwipeRepository, EfCoreSwipeRepository>();
builder.Services.AddScoped<ISwipeManager, SwipeManager>();

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
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Match Microservice API", Version = "v1" });
});

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