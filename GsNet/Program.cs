using Microsoft.EntityFrameworkCore;
using GsNetApi.Data;
using GsNetApi.Services;
using GsNetApi.Services.Interfaces;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Logging enxuto
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Controllers
builder.Services.AddControllers();

// Versionamento
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";                // v1, v2
    options.SubstituteApiVersionInUrl = true;          // substitui {version}
});

// Explorer para Swagger
builder.Services.AddEndpointsApiExplorer();

// Swagger com múltiplas versões
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "GsNet API V1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "GsNet API V2", Version = "v2" });
});

// OpenTelemetry (mais limpo)
builder.Services.AddOpenTelemetry()
    .WithTracing(tp =>
    {
        tp.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GsNetApi"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
        //.AddConsoleExporter();
    });

// Banco Oracle
var connectionString = builder.Configuration.GetConnectionString("OracleConnection")
    ?? throw new InvalidOperationException("Connection string 'OracleConnection' não encontrada.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString, o =>
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
);

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILocalizacaoTrabalhoService, LocalizacaoTrabalhoService>();
builder.Services.AddScoped<IMensagemService, MensagemService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// HealthChecks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// Swagger com versionamento
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{desc.GroupName}/swagger.json",
                desc.GroupName.ToUpperInvariant()
            );
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoints Health
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => true });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecksUI(opt => opt.UIPath = "/health-ui");

// Controllers
app.MapControllers();

app.Run();
