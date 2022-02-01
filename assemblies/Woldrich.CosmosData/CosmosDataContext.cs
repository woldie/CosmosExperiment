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

    // TODO figure out how to differentiate between local development and TEST/PROD environment configs
    private static String LocalCosmosEndpoint = "https://cosmosdb:8081";

    public static async Task ResetDatabase() {
        using (var context = new CosmosDataContext())
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            LocalCosmosEndpoint, 
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", // hardcoded key to cosmos local db
            databaseName: "CosmosExperiment",
            options => {
                options.ConnectionMode(ConnectionMode.Gateway);
                options.LimitToEndpoint();
                options.HttpClientFactory(static () => new HttpClient(httpMessageHandler) { BaseAddress = new Uri(LocalCosmosEndpoint) });
            }
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entityTypeBuilder => {
            entityTypeBuilder.HasManualThroughput(400);          // set our capacity - should only be done at deployment time

            entityTypeBuilder.HasPartitionKey(o => o.HashKey);
            entityTypeBuilder.HasKey(o => new { o.HashKey, o.RangeKey });

            entityTypeBuilder
                .Property(b => b.FirstName)
                .IsRequired();

            entityTypeBuilder
                .Property(b => b.LastName)
                .IsRequired();
        });
    }
}
