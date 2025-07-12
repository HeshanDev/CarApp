using System.Reflection;

using CarApp.Api.Extensions;
using CarApp.Persistence;
using CarApp.Persistence.Contexts;
using CarApp.Persistence.Extensions;

using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

string logPath;

if (builder.Environment.IsDevelopment())
{
    // Local debug: absolute path on host machine (Windows example)
    logPath = @"C:\CarApp\Logs\log-.txt";
}
else
{
    // Production / Docker container path (Linux style)
    logPath = "/app/Logs/log-.txt";
}

// Configure Serilog before building the app
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithProcessId()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

// 👇 Required to generate OpenAPI/Swagger docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplicationServices(builder.Configuration); // MediatR etc.

// Register persistence layer with SQLite connection string from appsettings.json
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("RedisConnection"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 👇 These methods require Swashbuckle
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CarDbContext>();
    await DbInitializer.SeedAsync(dbContext);
}


// Global Exception Handler Middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = errorFeature?.Error;

        var logger = Log.ForContext("RequestPath", context.Request.Path)
                        .ForContext("RequestMethod", context.Request.Method)
                        .ForContext("RequestQueryString", context.Request.QueryString.ToString());

        if (exception != null)
        {
            logger.Error(exception, "Unhandled exception caught by global handler");
        }
        else
        {
            logger.Error("Unhandled exception caught by global handler with no exception object");
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Error = "An unexpected error occurred. Please contact support.",
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
