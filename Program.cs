using Microsoft.EntityFrameworkCore;
using SOAP.Data;
using SOAP.Repository;


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

builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITripLocationRepository, TripLocationRepository>();





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