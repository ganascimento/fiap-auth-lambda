using Amazon.Lambda.APIGatewayEvents;

namespace AuthLambda.Utils
{
    public static class HttpStatusResult
    {
        public static APIGatewayProxyResponse Ok(string? body = null) => new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            Body = body
        };

        public static APIGatewayProxyResponse BadRequest(string? body = null) => new APIGatewayProxyResponse
        {
            StatusCode = 400,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            Body = body
        };

        public static APIGatewayProxyResponse Unauthorized(string? body = null) => new APIGatewayProxyResponse
        {
            StatusCode = 401,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            Body = body
        };
    }
}