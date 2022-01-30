using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Woldrich.CosmosModel;

namespace Woldrich.CosmosData;

public class CosmosDataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    private static HttpMessageHandler httpMessageHandler = new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            "https://localhost:8081", 
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", // hardcoded key to cosmos local db
            databaseName: "CosmosExperiment",
            options => {
                options.ConnectionMode(ConnectionMode.Direct);
                options.LimitToEndpoint();
                options.HttpClientFactory(static () => new HttpClient(httpMessageHandler));
            }
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entityTypeBuilder => {
            entityTypeBuilder.HasManualThroughput(400);          // set our capacity - should only be done at deployment time

            entityTypeBuilder.HasPartitionKey(o => o.HashKey);
            entityTypeBuilder.HasKey(o => o.RangeKey);

            entityTypeBuilder
                .Property(b => b.FirstName)
                .IsRequired();

            entityTypeBuilder
                .Property(b => b.LastName)
                .IsRequired();
        });
    }
}
