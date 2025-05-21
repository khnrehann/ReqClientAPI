using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReqClientAPI.Helper;
using ReqClientAPI.Interfaces;
using ReqClientAPI.Services;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ReqresApiHelper>(context.Configuration.GetSection("ReqresApi"));
        services.AddMemoryCache();
        services.AddHttpClient<IExternalUserService, ExternalUserService>();
        services.AddTransient<IExternalUserService, ExternalUserService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

var service = host.Services.GetRequiredService<IExternalUserService>();

Console.WriteLine("Getting all users...");
var users = await service.GetAllUsersAsync();
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.First_Name} {user.Last_Name} - {user.Email}");
}

Console.WriteLine("\nGetting user by ID (2)...");
var user2 = await service.GetUserByIdAsync(2);
if (user2 != null)
{

    Console.WriteLine($"{user2.First_Name} {user2.Last_Name} - {user2.Email}");
}
else
{
    Console.WriteLine("User not found.");
}
