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
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = ConfigExtensions.JsonOptions.PropertyNameCaseInsensitive;
				options.JsonSerializerOptions.PropertyNamingPolicy = ConfigExtensions.JsonOptions.PropertyNamingPolicy;
				options.JsonSerializerOptions.WriteIndented = ConfigExtensions.JsonOptions.WriteIndented;
				options.JsonSerializerOptions.AllowTrailingCommas = ConfigExtensions.JsonOptions.AllowTrailingCommas;
				options.JsonSerializerOptions.NumberHandling = ConfigExtensions.JsonOptions.NumberHandling;
				options.JsonSerializerOptions.ReadCommentHandling = ConfigExtensions.JsonOptions.ReadCommentHandling;
				options.JsonSerializerOptions.PreferredObjectCreationHandling = ConfigExtensions.JsonOptions.PreferredObjectCreationHandling;
				ConfigExtensions.JsonOptions.Converters.ToList().ForEach(options.JsonSerializerOptions.Converters.Add);
			});
			builder.Services.AddSignalR().AddJsonProtocol(options => {
				options.PayloadSerializerOptions = ConfigExtensions.JsonOptions;
			});
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			var app = builder.Build();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI();
			app.MapControllers();
			app.MapGet("/healthy", () => Results.Ok());
			app.MapGet("/ready", async (IKernelMemoryService kernelMemoryService) => await kernelMemoryService.IsReadyAsync() ? Results.Ok() : Results.BadRequest());
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
