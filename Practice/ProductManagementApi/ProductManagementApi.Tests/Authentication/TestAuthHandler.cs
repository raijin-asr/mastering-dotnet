using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ProductManagementApi.Tests.Authentication;

//Instead of generating JWT tokens for every authorization test, make a fake authentication handler.
public class TestAuthHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //this logic for testing unauthenticated anonymous user
        if (Context.Request.Headers["Test-Unauthenticated"] == "true")
        {
            return Task.FromResult(
                AuthenticateResult.NoResult()); //It means this authentication handler did not authenticate the request.
        }

        //Instead of hardcoding the role, we'll allow the test to specify which role it wants.
        var role = Context.Request.Headers["Test-Role"].ToString();

        if (string.IsNullOrWhiteSpace(role))
        {
            role = "Customer";
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, role) 
            //new Claim(ClaimTypes.Role, "Customer") //Every request will automatically be authenticated as a Customer.
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(
            AuthenticateResult.Success(ticket));
    }
}