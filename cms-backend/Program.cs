using cms_backend.Data;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Controllers
builder.Services.AddControllersWithViews();

// Use the recommended method to register FluentValidation services
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// Use the recommended method to register validators
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


// EF Core DB Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api", () => Results.Ok(ApiResponse<string>.Ok("Hello, Welcome to CMS API")));

app.Run();