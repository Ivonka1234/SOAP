using Microsoft.EntityFrameworkCore;
using SOAP.Data;


var builder = WebApplication.CreateBuilder(args);


// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// CONTROLLERS
builder.Services.AddControllers();


// SWAGGER
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();





var app = builder.Build();


// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();