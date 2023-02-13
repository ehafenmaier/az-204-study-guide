using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Data;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

// Get CosmosDb endpoint and access key from configuration settings
var endpointUri = config["CosmosDB:Endpoint"];
var primaryKey = config["CosmosDB:AccessKey"];

// Create variables to hold database and container IDs
var databaseId = "az204Database";
var containerId = "az204Container";

try
{
    Console.WriteLine("Beginning operations...\n");
    
    // Create CosmosDb client
    var cosmosClient = new CosmosClient(endpointUri, primaryKey);
    
    // Create database
    Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
    Console.WriteLine("Created Database: {0}\n", database.Id);

    // Create container in the database
    Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/LastName", 500);
    Console.WriteLine("Created Container: {0}\n", container.Id);
}
catch (CosmosException cex)
{
    Console.WriteLine("{0} error occurred: {1}", cex.StatusCode, cex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("Error: {0}", ex.Message);
}
finally
{
    Console.WriteLine("End of program, press any key to exit.");
    Console.ReadKey(); 
}