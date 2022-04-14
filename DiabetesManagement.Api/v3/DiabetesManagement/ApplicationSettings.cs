﻿using Microsoft.Extensions.Configuration;

namespace DiabetesManagement;
public class ApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
        DefaultConnectionString = configuration.GetConnectionString("default");
    }
    public string DefaultConnectionString { get; }
    public byte[] PersonalDataServerKeyBytes => !string.IsNullOrWhiteSpace(ConfidentialServerKey) ? Convert.FromBase64String(ConfidentialServerKey!) : Array.Empty<byte>();
    public byte[] ConfidentialServerKeyBytes => !string.IsNullOrWhiteSpace(PersonalDataServerKey) ? Convert.FromBase64String(PersonalDataServerKey!) : Array.Empty<byte>();
    public byte[] ServerInitialVectorBytes => !string.IsNullOrWhiteSpace(ServerInitialVector) ? Convert.FromBase64String(ServerInitialVector!) : Array.Empty<byte>();
    public string? HashAlgorithm { get; set; }
    public string? Algorithm { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
    public string? PersonalDataServerKey { get; set; }
    public string? ConfidentialServerKey { get; set; }
    public string? ServerInitialVector { get; set; }
}
