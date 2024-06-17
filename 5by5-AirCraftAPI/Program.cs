using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using _5by5_AirCraftAPI.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<_5by5_AirCraftAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("_5by5_AirCraftAPIContext") ?? throw new InvalidOperationException("Connection string '_5by5_AirCraftAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
