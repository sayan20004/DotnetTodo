using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoDotNet.Models;
using TodoDotNet.Services.Interfaces;
using TodoDotNet.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Services --------------------

builder.Services.AddControllers();

// Swagger (NET 8 compatible)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5500",
                "http://127.0.0.1:5500",
                "https://dotnettodoapp.netlify.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// MongoDB config
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// App services
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// -------------------- Middleware --------------------

// IMPORTANT: listen on Render port
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://*:{port}");
}

// Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected error occurred."
        });
    });
});

// Swagger (enable in Production for Render)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS + CORS
app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");

// Controllers
app.MapControllers();

app.Run();