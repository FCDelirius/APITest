var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repository (in-memory for demo)
builder.Services.AddSingleton<WebApi.Repositories.IUserRepository, WebApi.Repositories.InMemoryUserRepository>();

// Configuration: API key for token auth (override via environment variable ASPNETCORE_APIKEY)
builder.Configuration["ApiKey"] ??= "dev-token";

var app = builder.Build();

// Configure middleware pipeline in required order:
// 1) Error handling
// 2) Authentication
// 3) Logging
app.UseMiddleware<WebApi.Middleware.ExceptionHandlingMiddleware>();
app.UseMiddleware<WebApi.Middleware.TokenAuthenticationMiddleware>();
app.UseMiddleware<WebApi.Middleware.RequestResponseLoggingMiddleware>();

// Configure the HTTP request pipeline and Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
