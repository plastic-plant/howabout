using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Howabout.Configuration;
using Howabout.Hubs;
using Howabout.Interfaces;
using Howabout.Repositories;
using Howabout.Services;
using Serilog;
using Serilog.Events;

namespace Howabout.Commands
{
	public class ConsoleCommandStart : IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;

        public ConsoleCommandStart(ConsoleStartupArguments args)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));            
        }

        public Task Verify()
		{
			return Task.CompletedTask;
		}

		public async Task Execute()
		{
			var builder = WebApplication.CreateBuilder(_args.Arguments.ToArray());
			builder.Host.UseSerilog(Log.Logger);
			builder.Services.Configure<ModelProviderOptions>(builder.Configuration.GetSection(ModelProviderOptions.Section));
			builder.Services.AddSingleton<IKernelMemoryService, KernelMemoryService>();
			builder.Services.AddSingleton<IDocumentCache, DocumentRepository>();
			builder.Services.AddSingleton<IConversationService, ConversationService>();
			builder.Services.AddSingleton<IShellCommand, ShellCommand>();
			builder.Services.AddSystemMetrics();
			builder.Services.AddControllers().WithJsonOptions(ConfigExtensions.JsonDefaults);
			builder.Services.AddSignalR().WithJsonOptions(ConfigExtensions.JsonDefaults);
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI();
			app.MapControllers();
			app.MapGet("/api/healthy", () => Results.Ok());
			app.MapGet("/api/ready", async (IKernelMemoryService kernelMemoryService) => await kernelMemoryService.IsReadyAsync() ? Results.Ok() : Results.BadRequest());
			app.MapHub<EventMessageHub>("/hubs/eventMessageHub");
			app.MapFallbackToFile("/index.html");
			app.UseKernelMemoryService();
			app.UseCors(cors => cors
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetIsOriginAllowed(origin => true)
				.AllowCredentials()
			);
			await app.RunAsync();
		}

	}
}
