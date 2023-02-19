using Microsoft.Extensions.Configuration;

namespace az_msal_auth;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var tenantId = config["MSAL:TenantId"];
        var clientId = config["MSAL:ClientId"];

        Console.WriteLine("Tenant ID: {0}", tenantId);
        Console.WriteLine("Client ID: {0}", clientId);
    }
}
