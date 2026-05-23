using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SOAP.Data;
using SOAP.Models;
using SOAP.Profiles;
using SOAP.Repository;
using SOAP.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= DATABASE =================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ================= IDENTITY =================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ================= CONTROLLERS =================
builder.Services.AddControllers();

// ================= SWAGGER (FIXED) =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer YOUR_TOKEN"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ================= REPOSITORIES =================
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITripLocationRepository, TripLocationRepository>();

// ================= SERVICES =================
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITripLocationService, TripLocationService>();
builder.Services.AddScoped<IItineraryService, ItineraryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILocationService, LocationService>();

// ================= AUTOMAPPER =================
builder.Services.AddAutoMapper(typeof(TravelProfile));

// ================= JWT AUTH =================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            )
        };
    });

// ================= AUTHORIZATION =================
builder.Services.AddAuthorization();

// ================= BUILD APP =================
var app = builder.Build();

// ================= SWAGGER =================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ================= MIDDLEWARE =================
app.UseHttpsRedirection();

app.UseAuthentication(); // MUST come before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();