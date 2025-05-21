using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using ReqClientAPI.Helper;
using ReqClientAPI.Models;
using ReqClientAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReqClientAPI.Tests.Services
{
    public class ExternalUserServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly IOptions<ReqresApiHelper> _options;
        private readonly ILogger<ExternalUserService> _logger;
        private readonly IMemoryCache _cache;

        public ExternalUserServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://reqres.in/")
            };

            _options = Options.Create(new ReqresApiHelper { BaseUrl = "https://reqres.in/api/" });
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ExternalUserService>();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new UserModel
            {
                Id = 2,
                Email = "janet.weaver@reqres.in",
                First_Name = "Janet",
                Last_Name = "Weaver",
            };

            var apiResponse = new APIResponseModel<UserModel> { Data = expectedUser };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(apiResponse)
            };

            _handlerMock
                .SetupRequest(HttpMethod.Get, "https://reqres.in/api/users/2")
                .ReturnsAsync(responseMessage);

            var service = new ExternalUserService(_httpClient, _options, _logger, _cache);

            // Act
            var user = await service.GetUserByIdAsync(2);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(expectedUser.Id, user.Id);
            Assert.Equal(expectedUser.Email, user.Email);
        }
    }

    // Simple helper extension for mocking request
    public static class HttpMessageHandlerExtensions
    {
        public static Mock<HttpMessageHandler> SetupRequest(this Mock<HttpMessageHandler> mockHandler, HttpMethod method, string requestUri)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method &&
                        req.RequestUri.ToString() == requestUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            return mockHandler;
        }

        public static Mock<HttpMessageHandler> ReturnsAsync(this Mock<HttpMessageHandler> mockHandler, HttpResponseMessage response)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            return mockHandler;
        }
    }
}
