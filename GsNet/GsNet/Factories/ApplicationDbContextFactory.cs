using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GsNetApi.Data;
using Oracle.EntityFrameworkCore;
using System.IO; 

namespace GsNetApi.Factories
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory(); 
            
            if (basePath.EndsWith("bin") || basePath.EndsWith("Debug") || basePath.EndsWith("Release"))
            {
                basePath = Path.GetFullPath(Path.Combine(basePath, "..", "..", ".."));
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false) 
                .Build();

            var connectionString = configuration.GetConnectionString("OracleConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A Connection String 'OracleConnection' não foi encontrada no appsettings.json durante o Design-Time.");
            }

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            builder.UseOracle(
                connectionString, 
                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );

            return new ApplicationDbContext(builder.Options);
        }
    }
}