using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Add controllers and EF DbContext
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Swagger/OpenAPI (Swashbuckle)
builder.Services.AddSwaggerGen();

// Configure EF Core: SQLite for Development, env var for Production
{
    var env = builder.Environment;
    string connectionString;
    if (env.IsDevelopment())
    {
        connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "products.db")}";
        builder.Services.AddDbContext<AspireApp_Productos.ApiService.Data.AppDbContext>(options =>
            options.UseSqlite(connectionString));
    }
    else
    {
        // In non-development environments use the configured connection string from configuration
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        // If the connection string seems to be for SQLite, use UseSqlite, otherwise assume SQL Server
        if (connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase) && connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
        {
            builder.Services.AddDbContext<AspireApp_Productos.ApiService.Data.AppDbContext>(options =>
                options.UseSqlite(connectionString));
        }
        else
        {
            // Default to SQL Server for production-style connection strings
            builder.Services.AddDbContext<AspireApp_Productos.ApiService.Data.AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}

// Allow CORS (frontend will set its ApiBaseUrl in configuration)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Use Swashbuckle UI in development
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
else
{
    // Optionally enable Swagger in non-development if environment variable explicitly enables it
    var swaggerEnabled = Environment.GetEnvironmentVariable("SWAGGER_ENABLED");
    if (!string.IsNullOrEmpty(swaggerEnabled) && swaggerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.UseCors("AllowWeb");

app.MapControllers();

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
