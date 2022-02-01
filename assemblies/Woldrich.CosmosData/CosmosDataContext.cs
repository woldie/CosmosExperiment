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

    // TODO differentiate between local development and TEST/PROD environment configs
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

                // TODO:  should be able to forego this on TEST/PROD - this is part of the relaxing of self-signed TLS in local development
                options.HttpClientFactory(static () => new HttpClient(httpMessageHandler) { BaseAddress = new Uri(LocalCosmosEndpoint) });
            }
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ///////////////////////
        // CUSTOMER ENTITY MAPPING

        modelBuilder.Entity<Customer>(entityTypeBuilder => {
            
            // ////////////////
            // CONTAINER CONFIG

            entityTypeBuilder
                .ToContainer("Customers")           // Container is analgous to Column Family in Cassandra or Table in Dynamo
                .HasManualThroughput(400)           // set our capacity - TODO should only be done at deployment time?

                .HasNoDiscriminator()               // this is only useful if every Entity gets its own Container, but you really should be
                                                    // doing that so you can set throughput independently for each entity based on its
                                                    // performance characteristics.

                // NOTE:  this is the trick in EntityFrameworkCore to allow queries against a specific partition when looking up a single
                //  row.  You designate a column to the partition key (HashKey) and associate the key with both the partition key AND the
                //  key (RangeKey).  Then, you use the DBSet.FindAsync to lookup a single row by partition key AND key for good query performance.
                .HasPartitionKey(o => o.HashKey)
                .HasKey(o => new { o.HashKey, o.RangeKey });

            // ////////////
            // SCHEMA RULES

            entityTypeBuilder
                .Property(b => b.FirstName)
                .IsRequired();

            entityTypeBuilder
                .Property(b => b.LastName)
                .IsRequired();
        });
    }
}
