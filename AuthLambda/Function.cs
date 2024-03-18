using System.Text.Json;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AuthLambda.Dtos;
using AuthLambda.Services;
using AuthLambda.Utils;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AuthLambda;

public class Function
{
    //deploy 
    
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
    {
        if (string.IsNullOrEmpty(apigProxyEvent.Body))
            return HttpStatusResult.BadRequest("Invalid request!");

        try
        {
            var auth = JsonSerializer.Deserialize<AuthDto>(apigProxyEvent.Body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (auth == null)
                return HttpStatusResult.BadRequest("Invalid body!");

            using (var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast2))
            {
                var userPoolService = new UserPoolService(provider);

                var userPoolId = await userPoolService.GetUserPoolId();
                var clientId = await userPoolService.GetUserPoolClientId(userPoolId);

                var userPool = new CognitoUserPool(userPoolId, clientId, provider);
                var user = new CognitoUser(auth.Cpf, clientId, userPool, provider);
                var authRequest = new InitiateAdminNoSrpAuthRequest { Password = auth.Cpf };
                var authResponse = await user.StartWithAdminNoSrpAuthAsync(authRequest);

                if (authResponse.AuthenticationResult != null)
                {
                    return HttpStatusResult.Ok(JsonSerializer.Serialize(new
                    {
                        Token = authResponse.AuthenticationResult.AccessToken,
                        ExpiresIn = authResponse.AuthenticationResult.ExpiresIn
                    }));
                }

                return HttpStatusResult.Unauthorized();
            }
        }
        catch (Exception ex)
        {
            return HttpStatusResult.BadRequest(ex.Message);
        }
    }
}
