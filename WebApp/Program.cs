using System.Net;
using System.Text.Json.Serialization;
using Application;
using Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Persistence;
using WebApp;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 🔧 Configure Services
// ----------------------------

// Controllers + enum serialization
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger for API docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.CustomSchemaIds(type => type.FullName!.Replace("+", "."));
});

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                return uri.Host == "localhost";
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// EF Core: SQLite connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=videogames.db"));

// Application layer (MediatR, Pipelines, Validators)
builder.Services.RegisterApplication(builder.Configuration);

// ----------------------------
// Build and Configure App
// ----------------------------
WebApplication app = builder.Build();

// Global exception handler
app.UseExceptionHandler(err => err.Run(HandleExceptionsAsync));

// Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");

app.UseAuthorization();
app.MapControllers();

// ----------------------------
// Seed Database
// ----------------------------
await DbSeeder.SeedAsync(
    app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>()
);

// ----------------------------
// Run App
// ----------------------------
app.Run();

// ----------------------------
// Global Exception Handler
// ----------------------------
static async Task HandleExceptionsAsync(HttpContext context)
{
    IExceptionHandlerPathFeature? exceptionDetails = context.Features.Get<IExceptionHandlerPathFeature>();
    Exception? exception = exceptionDetails?.Error;

    if (exception is AggregateException aggEx)
        exception = aggEx.InnerExceptions.First();

    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

    switch (exception)
    {
        case ValidationException validationEx:
            await context.Response.WriteAsJsonAsync(new { validationEx.Message, validationEx.Errors });
            break;

        case AppException appEx:
            await context.Response.WriteAsJsonAsync(new { appEx.Message, appEx.Payload });
            break;

        default:
            await context.Response.WriteAsJsonAsync(new { error = exception?.Message });
            break;
    }
}
