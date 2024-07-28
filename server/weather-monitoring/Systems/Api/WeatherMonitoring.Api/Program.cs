using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.Api;
using WeatherMonitoring.Api.Configuration;
using WeatherMonitoring.Context;
using WeatherMonitoring.Context.Seeder;
using WeatherMonitoring.Services.Settings;
using WeatherMonitoring.Settings;
using WeatherMonitoring.Services.Logger;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddAppVersioning();
builder.Services.AddAppCors();
builder.Services.AddAppHealthChecks();
builder.Services.AddAppValidator();
builder.Services.AddAppSwagger(mainSettings, swaggerSettings);
builder.Services.AddAppAutoMappers();
builder.Services.AddAppControllers();
builder.Services.RegisterServices();

var app = builder.Build();

app.UseAppSwagger();
app.UseAppHealthChecks();
app.UseAppCors();
app.UseAppControllers();

DbInitializer.Execute(app.Services);
DbSeeder.Execute(app.Services);

var logger = app.Services.GetRequiredService<IAppLogger>();

logger.Information("The WeatherMonitoring.Api has started");

app.Run();

logger.Information("The WeatherMonitoring.Api has stopped");
