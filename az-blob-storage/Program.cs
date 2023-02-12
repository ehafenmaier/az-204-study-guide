using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;


Console.WriteLine("Azure Blob Storage exercise\n");

IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

// Get storage connection string from configuration settings
var storageConectionString = config.GetConnectionString("AzureStorage");

// Run the examples asynchronously, wait for the results before proceeding
ProcessAsync(storageConectionString).GetAwaiter().GetResult();

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();


static async Task ProcessAsync(string? storageConnectionString)
{
    // Create blob service client using storage connection string
    BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
    
    // Create a new uniquely named container
    var containerName = "wtblob-" + Guid.NewGuid();

    // Create the container and get the container client
    BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
    Console.WriteLine("A container named '{0}' has been created.", containerName);
    Console.WriteLine("Take a minute and verify in the portal."); 
    Console.WriteLine("Next a file will be created and uploaded to the container.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();
    
    // Create a local file in the ./data/ directory for uploading and downloading
    var localPath = "./data/";
    var fileName = "wtfile-" + Guid.NewGuid() + ".txt";
    var localFilePath = Path.Combine(localPath, fileName);

    // Write text to the file
    await File.WriteAllTextAsync(localFilePath, "Hello, World!");

    // Get a reference to the blob
    BlobClient blobClient = containerClient.GetBlobClient(fileName);

    Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

    // Open the file and upload its data
    await using (FileStream uploadFileStream = File.OpenRead(localFilePath))
    {
        await blobClient.UploadAsync(uploadFileStream);
        uploadFileStream.Close();
    }
    
    Console.WriteLine("\nThe file was uploaded. We'll verify by listing the blobs next.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();   
    
    // List blobs in the container
    Console.WriteLine("Listing blobs...");
    await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
    {
        Console.WriteLine("\t" + blobItem.Name);
    }

    Console.WriteLine("\nYou can also verify by looking inside the " + 
                      "container in the portal." +
                      "\nNext the blob will be downloaded with an altered file name.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();
    
    // Download the blob to a local file
    // Append the string "DOWNLOADED" before the .txt extension 
    string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

    Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

    // Download the blob's contents and save it to a file
    BlobDownloadInfo download = await blobClient.DownloadAsync();

    using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
    {
        await download.Content.CopyToAsync(downloadFileStream);
    }
    
    Console.WriteLine("\nLocate the local file in the data directory created earlier to verify it was downloaded.");
    Console.WriteLine("The next step is to delete the container and local files.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();
    
    // Delete the container and clean up local files created
    Console.WriteLine("\n\nDeleting blob container...");
    await containerClient.DeleteAsync();

    Console.WriteLine("Deleting the local source and downloaded files...");
    File.Delete(localFilePath);
    File.Delete(downloadFilePath);

    Console.WriteLine("Finished cleaning up.");
}
