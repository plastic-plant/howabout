using Howabout.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Howabout.Configuration
{
    public static class ConfigExtensions
    {
        public static IApplicationBuilder UseKernelMemoryService(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IKernelMemoryService>();
                service.Configure();
            }

            return app;
        }

		public static JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
		{
			WriteIndented = true,
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true,
			NumberHandling = JsonNumberHandling.AllowReadingFromString,
			ReadCommentHandling = JsonCommentHandling.Skip,
			PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
			Converters =
			{
				new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
			}
		};
	}
}
