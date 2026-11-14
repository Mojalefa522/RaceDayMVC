using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace RaceDayAPI.Tests
{
    public class RoleTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public RoleTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Unauthenticated_CreateEvent_Returns401()
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("Test Event"), "Name");
            formData.Add(new StringContent("Test Description"), "Description");
            formData.Add(new StringContent("Test Location"), "Location");
            formData.Add(new StringContent("2027-01-01T10:00:00"), "StartDate");
            formData.Add(new StringContent("2027-01-01T12:00:00"), "EndDate");

            var response = await _client.PostAsync("/api/events", formData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}