using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace az_msal_auth;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create configuration settings
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var tenantId = config["MSAL:TenantId"];
        var clientId = config["MSAL:ClientId"];

        Console.WriteLine("Tenant ID: {0}", tenantId);
        Console.WriteLine("Client ID: {0}", clientId);
        
        // Create public client application
        var app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithRedirectUri("http://localhost")
            .Build();
        
        // Set authentication scopes
        string[] scopes = { "user.read" };
        
        // Acquire access token
        var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

        Console.WriteLine("Access Token: {0}", result.AccessToken);
    }
}
