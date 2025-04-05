using Amazon;
using TaskManagementApi.Settings;
using Microsoft.Extensions.Options;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace TaskManagementApi.Services;

public class AuthService
{
    private readonly AmazonCognitoIdentityProviderClient _cognito;
    private readonly string _clientId;

    public AuthService(IOptions<CognitoSettings> config)
    {
        _clientId = config.Value.ClientId;
        _cognito = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
    }

    public async Task<AuthenticationResultType?> SignInAsync(string username, string password, string? newPassword = null)
    {
        var request = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            ClientId = _clientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", username },
                { "PASSWORD", password }
            }
        };

        var response = await _cognito.InitiateAuthAsync(request);

        if (response.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new Exception("New password required but not provided.");

            var challengeRequest = new RespondToAuthChallengeRequest
            {
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                ClientId = _clientId,
                Session = response.Session,
                ChallengeResponses = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "NEW_PASSWORD", newPassword }
                }
            };

            var challengeResponse = await _cognito.RespondToAuthChallengeAsync(challengeRequest);
            return challengeResponse.AuthenticationResult;
        }

        return response.AuthenticationResult;
    }

    public async Task SignUpAsync(string email, string password)
    {
        var request = new SignUpRequest
        {
            ClientId = _clientId,
            Username = email,
            Password = password,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email }
            }
        };

        await _cognito.SignUpAsync(request);
    }
}
