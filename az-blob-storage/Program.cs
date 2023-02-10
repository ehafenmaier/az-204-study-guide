using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;


Console.WriteLine("Azure Blob Storage exercise\n");

IConfiguration config = new ConfigurationBuilder()
    
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

// Run the examples asynchronously, wait for the results before proceeding
ProcessAsync(config).GetAwaiter().GetResult();

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();


static async Task ProcessAsync(IConfiguration config)
{
    // Get storage connection string from configuration settings
    var storageConectionString = config.GetConnectionString("AzureStorage");
    
    // Create blob service client using storage connection string
    var blobServiceClient = new BlobServiceClient(storageConectionString);
}
