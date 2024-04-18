using Howabout.Interfaces;

namespace Howabout.Configuration
{
    public static class ConfigurationExtensions
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
    }
}
