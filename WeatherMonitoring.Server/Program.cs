using NLog;
using WeatherMonitoring.Server.Extensions;
using WeatherMonitoring.Server;
using WeatherMonitoring.Repository;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherMonitoring.Server.Presentation.ActionFilters;
using WeatherMonitoring.MonitoringService;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureCors();
builder.Services.ConfigureAPIVersioning();
builder.Services.ConfigureRateLimiter(builder.Configuration);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureWeatherAPI(builder.Configuration);
builder.Services.ConfigureSqlContextFactory(builder.Configuration);
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSwagger();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
})
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(WeatherMonitoring.Server.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

RepositoryInitializer.Execute(app.Services);
RepositorySeeder.Init(app.Services, initWeatherFromCsv: true);

MonitoringManager.StartMonitoring(app.Services, app.Configuration);

app.UseSwagger();

app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();
