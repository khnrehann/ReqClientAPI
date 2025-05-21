using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using ReqClientAPI.Helper;
using ReqClientAPI.Models;
using System.Net.Http.Json;

namespace ReqClientAPI.Interfaces
{
    public interface IExternalUserService
    {
        Task<UserModel?> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserModel>> GetAllUsersAsync(); // ✅ correct
    }
}
