using Howabout.Interfaces;
using Howabout.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Howabout.Configuration
{
	public static class ConfigExtensions
    {
		public static IServiceCollection AddSystemMetrics(this IServiceCollection services)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
			{
				return services.AddScoped<ISystemRepository, SystemFreeBsdRepository>();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return services.AddScoped<ISystemRepository, SystemLinuxRepository>();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return services.AddScoped<ISystemRepository, SystemMacOSRepository>();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return services.AddScoped<ISystemRepository, SystemWindowsRepository>();
			}
			else throw new NotImplementedException(RuntimeInformation.OSDescription);
		}

		public static JsonSerializerOptions JsonDefaults = new(JsonSerializerDefaults.Web)
		{
			WriteIndented = true,
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			AllowTrailingCommas = true,
			NumberHandling = JsonNumberHandling.AllowReadingFromString,
			ReadCommentHandling = JsonCommentHandling.Skip,
			PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
			Converters =
			{
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
			}
		};

		public static IMvcBuilder WithJsonOptions(this IMvcBuilder builder, JsonSerializerOptions config)
		{
			return builder.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = config.PropertyNameCaseInsensitive;
				options.JsonSerializerOptions.PropertyNamingPolicy = config.PropertyNamingPolicy;
				options.JsonSerializerOptions.WriteIndented = config.WriteIndented;
				options.JsonSerializerOptions.AllowTrailingCommas = config.AllowTrailingCommas;
				options.JsonSerializerOptions.NumberHandling = config.NumberHandling;
				options.JsonSerializerOptions.ReadCommentHandling = config.ReadCommentHandling;
				options.JsonSerializerOptions.PreferredObjectCreationHandling = config.PreferredObjectCreationHandling;
				config.Converters.ToList().ForEach(options.JsonSerializerOptions.Converters.Add);
			});
		}

		public static ISignalRServerBuilder WithJsonOptions(this ISignalRServerBuilder builder, JsonSerializerOptions config)
		{
			return builder.AddJsonProtocol(options => {
				options.PayloadSerializerOptions = config;
			});
		}

		public static IApplicationBuilder UseKernelMemoryService(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IKernelMemoryService>();
                service.Configure();
            }

            return app;
        }
	}
}
