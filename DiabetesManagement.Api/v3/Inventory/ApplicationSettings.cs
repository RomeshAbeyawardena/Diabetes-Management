using Inventory.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Inventory;
public class ApplicationSettings
{
    private string? systemAdministratorUser;
    public ApplicationSettings(IConfiguration configuration, ILogger<ApplicationSettings> logger)
    {
        configuration.Bind(this);
        DefaultConnectionString = configuration.GetConnectionString("default");

        LedgerConnectionString = configuration.GetConnectionString("ledger");

        if (EnableSystemAdmin)
        {
            logger
                .LogWarning($"System Admin API Key: {SystemAdministratorUser}{Environment.NewLine}Should not be used to authenticate application endpoints!");
        }
    }
    public string SystemAdministratorUser => systemAdministratorUser ??= Guid.NewGuid().ToString("D");
    public bool DiscoveryMode { get; set; }
    public TimeSpan? DefaultApplicationExpiry { get; set; }
    public string DefaultConnectionString { get; }
    public string LedgerConnectionString { get; }
    public byte[] PersonalDataServerKeyBytes => !string.IsNullOrWhiteSpace(ConfidentialServerKey) 
        ? Convert.FromBase64String(ConfidentialServerKey!) 
        : Array.Empty<byte>();

    public byte[] ConfidentialServerKeyBytes => !string.IsNullOrWhiteSpace(PersonalDataServerKey) 
        ? Convert.FromBase64String(PersonalDataServerKey!) 
        : Array.Empty<byte>();

    public byte[] ServerInitialVectorBytes => !string.IsNullOrWhiteSpace(ServerInitialVector) 
        ? Convert.FromBase64String(ServerInitialVector!) 
        : Array.Empty<byte>();

    public TimeSpan SessionExpiry { get; set; }
    public string? HashAlgorithm { get; set; }
    public string? Algorithm { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
    public string? PersonalDataServerKey { get; set; }
    public string? ConfidentialServerKey { get; set; }
    public string? ServerInitialVector { get; set; }
    public bool EnableSystemAdmin { get; set; }
}
