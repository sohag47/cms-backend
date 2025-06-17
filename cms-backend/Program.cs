using cms_backend.Data;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FluentValidation; // Add this namespace for AddValidatorsFromAssemblyContaining

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();

// Use the recommended method to register FluentValidation services
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

// Use the recommended method to register validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
