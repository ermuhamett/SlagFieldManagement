using SlagFieldManagement.Api.Extensions;
using SlagFieldManagement.Application;
using SlagFieldManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Добавление CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL Next.js-приложения
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    //await app.ApplyMigrations();
    //app.TruncateData();
    //app.SeedData();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

// Включение CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
