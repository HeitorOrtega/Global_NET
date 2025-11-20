using Microsoft.EntityFrameworkCore;
using GsNetApi.Data;
using GsNetApi.Services;
using GsNetApi.Services.Interfaces;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Logging
// -------------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// -------------------------------
// Controllers + Swagger
// -------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------------
// OpenTelemetry Tracing
// -------------------------------
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GsNetApi"))
            .AddAspNetCoreInstrumentation()    
            .AddHttpClientInstrumentation()    
            .AddConsoleExporter();            
    });

// -------------------------------
// API Versioning
// -------------------------------
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); 
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader()
    );
});

// -------------------------------
// Banco Oracle com EF Core
// -------------------------------
var connectionString = builder.Configuration.GetConnectionString("OracleConnection")
    ?? throw new InvalidOperationException("Connection string 'OracleConnection' não encontrada.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
);

// -------------------------------
// Dependency Injection
// -------------------------------
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILocalizacaoTrabalhoService, LocalizacaoTrabalhoService>();
builder.Services.AddScoped<IMensagemService, MensagemService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// -------------------------------
// Health Checks
// -------------------------------
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

// -------------------------------
// Swagger UI
// -------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -------------------------------
// HTTPS + Authorization
// -------------------------------
app.UseHttpsRedirection();
app.UseAuthorization();

// -------------------------------
// Health Checks endpoints
// -------------------------------
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => true });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecksUI(options => options.UIPath = "/health-ui");

// -------------------------------
// Controllers
// -------------------------------
app.MapControllers();

app.Run();
