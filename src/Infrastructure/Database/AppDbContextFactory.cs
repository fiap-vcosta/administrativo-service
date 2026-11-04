using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Infrastructure.Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "administrativo",
                Username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres",
                MaxPoolSize = 15
            };
            var pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            if (!string.IsNullOrEmpty(pwd))
            {
                builder.Password = pwd;
            }

            connectionString = builder.ConnectionString;
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}
