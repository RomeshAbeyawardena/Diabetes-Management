using MediatR;

namespace DiabetesManagement.Features.AccessToken;

public class SignRequest : IRequest<string>
{
    public string? ApiKey { get; set; }
    public string? ApiChallenge { get; set; }
}
