using Microsoft.EntityFrameworkCore;
using GsNetApi.Data;
using GsNetApi.Services;
using GsNetApi.Services.Interfaces;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader()
    );
}).AddMvc();

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A Connection String 'OracleConnection' não foi encontrada no appsettings.json.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
);

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILocalizacaoTrabalhoService, LocalizacaoTrabalhoService>();
builder.Services.AddScoped<IMensagemService, MensagemService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(30);
    options.AddHealthCheckEndpoint("GsNet API", "/health");
}).AddInMemoryStorage();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecksUI(options => options.UIPath = "/health-ui");

app.MapControllers();

app.Run();