using Amazon;
using TaskManagementApi.Settings;
using Microsoft.Extensions.Options;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Logging;

namespace TaskManagementApi.Services;

public class AuthService
{
    private readonly AmazonCognitoIdentityProviderClient _cognito;
    private readonly string _clientId;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IOptions<CognitoSettings> config, ILogger<AuthService> logger)
    {
        _clientId = config.Value.ClientId;
        _logger = logger;
        _cognito = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
    }

    public async Task<AuthenticationResultType?> SignInAsync(string username, string password, string? newPassword = null)
    {
        _logger.LogInformation("Attempting sign-in for user: {Username}", username);

        try
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
                _logger.LogWarning("User {Username} requires a new password", username);

                if (string.IsNullOrEmpty(newPassword))
                {
                    _logger.LogError("New password required but not provided for user {Username}", username);
                    throw new Exception("New password required but not provided.");
                }

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
                _logger.LogInformation("Password updated successfully for user: {Username}", username);
                return challengeResponse.AuthenticationResult;
            }

            _logger.LogInformation("User {Username} signed in successfully", username);
            return response.AuthenticationResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sign-in failed for user: {Username}", username);
            throw;
        }
    }

    public async Task SignUpAsync(string email, string password)
    {
        _logger.LogInformation("Attempting sign-up for user: {Email}", email);

        try
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
            _logger.LogInformation("Sign-up successful for user: {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sign-up failed for user: {Email}", email);
            throw;
        }
    }
}
