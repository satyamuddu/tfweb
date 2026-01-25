
using Serilog;
using System.Reflection;
using TransmissionFacilityWebApp.Core.Interfaces;
using TransmissionFacilityWebApp.DBContext;
using TransmissionFacilityWebApp.Repository;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Host.UseSerilog();
Log.Information("Starting up the application...");

//dependency injections would go here
builder.Services.AddScoped<ITransmissionFacilityRepository, TransmissionFacilityRepository>();
builder.Services.AddScoped<IRatingProposalRepository, RatingProposalRepository>();

//mediatR registration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(TransmissionFacilityWebApp.Application.Queries.GetTransmissionFacilitiesQuery)));

// Add db context registration
builder.Services.AddDbContext<TransmissionFacilityDbContext>();
builder.Services.AddDbContext<RatingProposalDbContext>();

builder.Services.AddControllers();
// Add services to the container.
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
app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
