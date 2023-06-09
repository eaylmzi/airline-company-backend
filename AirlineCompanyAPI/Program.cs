using airlinecompany.Data.Models;
using airlinecompany.Data.Repositories.Companies;
using airlinecompany.Data.Repositories.FlightAttendants;
using airlinecompany.Data.Repositories.Flights;
using airlinecompany.Data.Repositories.Passengers;
using airlinecompany.Data.Repositories.Planes;
using airlinecompany.Data.Repositories.Points;
using airlinecompany.Data.Repositories.SessionPassengers;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Logic.Logics.Flights;
using airlinecompany.Logic.Logics.JoinTables;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using airlinecompany.Logic.Logics.Points;
using airlinecompany.Logic.Logics.SessionPassengers;
using AirlineCompanyAPI.Services.Cipher;
using AirlineCompanyAPI.Services.Jwt;
using AirlineCompanyAPI.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICipherService, CipherService>();
builder.Services.AddScoped<IJoinTableLogic, JoinTableLogic>();


builder.Services.AddScoped<ICompanyLogic, CompanyLogic>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IFlightAttendantLogic, FlightAttendantLogic>();
builder.Services.AddScoped<IFlightAttendantRepository, FlightAttendantRepository>();

builder.Services.AddScoped<IFlightLogic, FlightLogic>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddScoped<IPassengerLogic, PassengerLogic>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();

builder.Services.AddScoped<IPlaneLogic, PlaneLogic>();
builder.Services.AddScoped<IPlaneRepository, PlaneRepository>();

builder.Services.AddScoped<IPointLogic, PointLogic>();
builder.Services.AddScoped<IPointRepository, PointRepository>();

builder.Services.AddScoped<ISessionPassengerLogic, SessionPassengerLogic>();
builder.Services.AddScoped<ISessionPassengerRepository, SessionPassengerRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme(\"bearer{token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value ?? throw new ArgumentNullException())),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
        });
});
var app = builder.Build();
app.UseApiVersioning();
// Configure the HTTP request pipeline.




app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseAuthorization();


app.MapControllers();

app.Run();
