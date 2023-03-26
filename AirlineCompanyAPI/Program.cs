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
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using airlinecompany.Logic.Logics.Points;
using airlinecompany.Logic.Logics.SessionPassengers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
