using Microsoft.EntityFrameworkCore;
using ShopSmartAPI.Data;
using ShopSmartAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Example: Add other logging providers if needed
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware for generating Swagger JSON
    app.UseSwaggerUI(); // Enable Swagger UI for visualizing and interacting with the API

    // In production, these should be guarded with proper configurations for security
    app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
}

app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Map controllers

app.Run();