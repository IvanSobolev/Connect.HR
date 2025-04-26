using System.Text;
using AuthMicroservice.Models.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using TinderAPI.Models;
using TinderAPI.Repositories.Implementations;
using TinderAPI.Repositories.Interfaces;
using TinderAPI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AuthOptions authOptions = new AuthOptions(Environment.GetEnvironmentVariable("AUTH_OPTIONS") ??
                                          throw new InvalidOperationException(
                                              "AuthOptions: authOptions is not configured"));


var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"),
        x => x.UseNetTopologySuite()
        ));

builder.Services.AddSingleton<GeometryFactory>(
    provider => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddScoped<IProfileRepository, EfCoreProfileRepository>();
builder.Services.AddScoped<IPreferencesRepository, EfCorePreferencesRepository>();
builder.Services.AddScoped<IPhotoRepository, EfCorePhotoRepository>();
builder.Services.AddScoped<IHobbyRepository, EfCoreHobbyRepository>();

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
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Auth Microservice API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProfileGrpcService>();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.Run();