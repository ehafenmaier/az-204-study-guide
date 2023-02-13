using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

// Replace <documentEndpoint> with the information created earlier
var EndpointUri = config["CosmosDB:Endpoint"];

// Set variable to the Primary Key from earlier.
var PrimaryKey = config["CosmosDB:AccessKey"];
