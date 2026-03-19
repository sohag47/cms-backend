using Core.Data;
using Core.Models.Base;
using Core.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Controllers
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Custom API response for validation errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key.Split('.').Last(),
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        var response = ApiResponse<string>.Fail("Validation failed.", errors);
        return new UnprocessableEntityObjectResult(response);
    };
});

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.WithOrigins("https://localhost:5002")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Auto-create DB and apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AdminPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

// app.MapGet("/", () => Results.Redirect("/api"));
app.MapGet("/", () => Results.Ok(ApiResponse<string>.Ok("Hello, Welcome to CMS API")));

app.Run();