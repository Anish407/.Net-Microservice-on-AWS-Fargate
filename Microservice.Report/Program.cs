using Microservice.Report.BusinessLogic;
using Microservice.Report.Config;
using Microservice.Report.Infra.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddDbContextPool<WeatherReportContext>(op =>
{
    op.EnableSensitiveDataLogging();
    op.EnableDetailedErrors();
    op.UseNpgsql(config.GetConnectionString("AppDb"));
});

builder.Services.Configure<WeatherDataConfig>(config.GetSection("WeatherDataConfig"));
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
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
