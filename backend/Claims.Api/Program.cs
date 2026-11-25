using System.Text.Json.Serialization;
using Claims.Api.Data;
using Claims.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register controllers and JSON settings
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

// Register OpenAPI
builder.Services.AddOpenApi();

// Register database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// Register claim services
builder.Services.AddScoped<IClaimService, ClaimService>();

// Configure Angular CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Development environment configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Populate fictional development users
    await DevelopmentDataSeeder.SeedAsync(app.Services);
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.MapControllers();

// Health check
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        application = "Claims Workflow API"
    });
});

app.Run();