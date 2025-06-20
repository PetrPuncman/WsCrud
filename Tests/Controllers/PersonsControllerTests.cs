using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WsCrud.Models;
using Xunit;

namespace WsCrud.Tests.Controllers
{
    public class PersonsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PersonsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:password123"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }

        [Fact]
        public async Task GET_ReturnsEmptyListInitially()
        {
            var response = await _client.GetAsync("/api/persons");
            response.EnsureSuccessStatusCode();

            var people = await response.Content.ReadFromJsonAsync<List<Person>>();
            Assert.NotNull(people);
        }

        [Fact]
        public async Task UnauthorizedRequest_Returns401()
        {
            var unauthClient = new WebApplicationFactory<Program>().CreateClient();
            var response = await unauthClient.GetAsync("/api/persons");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}