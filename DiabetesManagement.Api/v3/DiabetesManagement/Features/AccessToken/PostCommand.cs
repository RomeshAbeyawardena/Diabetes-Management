using DiabetesManagement.Attributes;

namespace DiabetesManagement.Features.AccessToken
{
    [RequiresClaims(Permissions.AccessToken_View)]
    public class PostCommand
    {

    }
}
