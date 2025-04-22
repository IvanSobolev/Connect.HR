using Microsoft.AspNetCore.Server.Kestrel.Core;
using TinderAPI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGrpcService<UserGrpcService>();
app.MapControllers();

app.UseHttpsRedirection();
app.Run();