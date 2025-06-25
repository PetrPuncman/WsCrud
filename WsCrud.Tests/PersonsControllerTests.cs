using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WsCrud.Models;
using Xunit;

namespace WsCrud.Tests.Controllers
{
	/// <summary>
	/// Integration tests for the PersonsController, validating public API and authentication.
	/// </summary>
	public class PersonsControllerTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		/// <summary>
		/// Initializes the test client and applies Basic Auth credentials from environment variables.
		/// </summary>
		/// <param name="factory">The test application factory.</param>
		public PersonsControllerTests(WebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient();

			// Retrieve credentials from environment variables
			var username = Environment.GetEnvironmentVariable("AUTH_USERNAME") ?? "test-user";
			var password = Environment.GetEnvironmentVariable("AUTH_PASSWORD") ?? "test-pass";

			var raw = $"{username}:{password}";
			var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(raw));

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
		}

		/// <summary>
		/// Verifies that the initial GET request returns an empty list of persons.
		/// </summary>
		[Fact]
		public async Task GET_ReturnsEmptyListInitially()
		{
			var response = await _client.GetAsync("/api/persons");
			response.EnsureSuccessStatusCode();

			var people = await response.Content.ReadFromJsonAsync<List<Person>>();
			Assert.NotNull(people);
		}

		/// <summary>
		/// Verifies that unauthenticated requests receive a 401 Unauthorized response.
		/// </summary>
		[Fact]
		public async Task UnauthorizedRequest_Returns401()
		{
			var unauthClient = new WebApplicationFactory<Program>().CreateClient();
			var response = await unauthClient.GetAsync("/api/persons");

			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}