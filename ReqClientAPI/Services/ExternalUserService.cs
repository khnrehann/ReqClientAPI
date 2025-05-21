using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReqClientAPI.Helper;
using ReqClientAPI.Interfaces;
using ReqClientAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReqClientAPI.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ReqresApiHelper _options;
        private readonly ILogger<ExternalUserService> _logger;
        private readonly IMemoryCache _cache;

        public ExternalUserService(
            HttpClient httpClient,
            IOptions<ReqresApiHelper> options,
            ILogger<ExternalUserService> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
            _cache = cache;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await _cache.GetOrCreateAsync($"user_{userId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var url = $"{_options.BaseUrl}users/{userId}";
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("User {UserId} not found", userId);
                    return null;
                }

                response.EnsureSuccessStatusCode();

                // Read and return directly — don't store the response/content elsewhere
                var apiResult = await response.Content.ReadFromJsonAsync<APIResponseModel<UserModel>>();

                return apiResult.Data;
            });
        }


        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _cache.GetOrCreateAsync("all_users", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var allUsers = new List<UserModel>();
                int page = 1;
                int totalPages;

                do
                {
                    var url = $"{_options.BaseUrl}users?page={page}";
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<APIResponseListModel<UserModel>>();
                    if (result?.Data != null)
                        allUsers.AddRange(result.Data);

                    totalPages = result?.TotalPages ?? 0;
                    page++;
                } while (page <= totalPages);

                return allUsers;
            });
        }
    }
}
