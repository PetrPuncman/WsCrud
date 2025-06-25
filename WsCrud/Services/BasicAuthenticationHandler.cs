using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WsCrud.Services
{
	/// <summary>
	/// Handles HTTP Basic authentication using credentials from environment variables.
	/// </summary>
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		/// <summary>
		/// Constructs the BasicAuthenticationHandler with required dependencies.
		/// </summary>
		public BasicAuthenticationHandler(
				IOptionsMonitor<AuthenticationSchemeOptions> options,
				ILoggerFactory logger,
				System.Text.Encodings.Web.UrlEncoder encoder,
				ISystemClock clock)
				: base(options, logger, encoder, clock) { }

		/// <summary>
		/// Handles authentication for each incoming request.
		/// Validates the "Authorization" header against username and password loaded from environment variables.
		/// </summary>
		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			// Check if Authorization header exists
			if (!Request.Headers.ContainsKey("Authorization"))
				return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));

			try
			{
				// Decode base64 credentials
				var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
				var credentials = Encoding.UTF8
						.GetString(Convert.FromBase64String(authHeader.Parameter ?? ""))
						.Split(':', 2);

				if (credentials.Length != 2)
					return Task.FromResult(AuthenticateResult.Fail("Invalid credential format"));

				var username = credentials[0];
				var password = credentials[1];

				// Read expected credentials from environment
				var expectedUsername = Environment.GetEnvironmentVariable("AUTH_USERNAME");
				var expectedPassword = Environment.GetEnvironmentVariable("AUTH_PASSWORD");

				// Validate presence of expected credentials
				if (string.IsNullOrWhiteSpace(expectedUsername) || string.IsNullOrWhiteSpace(expectedPassword))
					return Task.FromResult(AuthenticateResult.Fail("Authentication not configured"));

				// Validate user credentials
				if (username != expectedUsername || password != expectedPassword)
					return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));

				// Build ClaimsPrincipal if valid
				var claims = new[] { new Claim(ClaimTypes.Name, username) };
				var identity = new ClaimsIdentity(claims, Scheme.Name);
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, Scheme.Name);

				return Task.FromResult(AuthenticateResult.Success(ticket));
			}
			catch
			{
				return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
			}
		}
	}
}