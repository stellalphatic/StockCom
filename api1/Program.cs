using api1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using api1.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var ConnString = builder.Configuration.GetConnectionString("DefaultConnection");


// Adding DB connection before builder.build()
builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseMySql(ConnString, ServerVersion.AutoDetect(ConnString))
     );

builder.Services.AddControllers();

builder.Services.AddScoped<StockRepository>();
builder.Services.AddScoped<CommentRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//just before app.Run
app.MapControllers();

app.Run();
