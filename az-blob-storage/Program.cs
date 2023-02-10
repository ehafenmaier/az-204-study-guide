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
    BlobServiceClient blobServiceClient = new BlobServiceClient(storageConectionString);
    
    // Create a new uniquely named container
    var containerName = $"wtblob-{Guid.NewGuid()}";
    
    // Create the container and get the container client
    BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
    Console.WriteLine("A container named '{0}' has been created.", containerName);
    Console.WriteLine("Take a minute and verify in the portal."); 
    Console.WriteLine("Next a file will be created and uploaded to the container.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();
}
