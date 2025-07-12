using System.Reflection;

using CarApp.Api.Extensions;
using CarApp.Persistence;
using CarApp.Persistence.Contexts;
using CarApp.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
