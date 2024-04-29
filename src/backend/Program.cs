
using Howabout.Configuration;
using Howabout.Controllers;
using Howabout.Extensions;
using Howabout.Hubs;
using Howabout.Interfaces;
using Howabout.Repositories;
using Howabout.Services;
using System.Text.Json;

namespace Howabout
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var cli = new CommandLineStartupArguments(args);
			switch (cli.Command)
			{
				case CommandLineStartupArguments.CommandArg.Help:
					Console.WriteLine("Help information.");
					break;

				case CommandLineStartupArguments.CommandArg.Version:
					Console.WriteLine("Version information.");
					break;

				case CommandLineStartupArguments.CommandArg.Start:
					var builder = WebApplication.CreateBuilder(args);
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

					// ConfigExtensions.JsonOptions
					);
					await app.RunAsync();
					break;

				case CommandLineStartupArguments.CommandArg.Stop:
					Console.WriteLine("Stop command.");
					using (var client = new HttpClient())
					{
						var response = await client.GetAsync("http://localhost:5153/configuration/stop");
						if (response.IsSuccessStatusCode)
						{
							Console.WriteLine("Stopped.");
						}
						else
						{
							Console.WriteLine("Failed to stop.");
						}
					};
					break;

				case CommandLineStartupArguments.CommandArg.Add:
					Console.WriteLine("Add command.");
					if (cli.Options.Count() == 0)
					{
						Console.WriteLine("No options specified for a path or url to add.");
						break;
					}

					var filenames = cli.Options
						.Except(cli.GetUrls())
						.ToFullPaths()
						.IncludeDirectoryFiles();

					// Request processing of uploaded files.
					foreach (var filepath in filenames)
					{
						using var client = new HttpClient();
						using (var multipart = new MultipartFormDataContent())
						{
							multipart.Add(JsonContent.Create(new DocumentAddRequest() { Tags = cli.GetTags() }, options: ConfigExtensions.JsonOptions), "json", "request.json");
							using (var binaryContent = new ByteArrayContent(await File.ReadAllBytesAsync(filepath)))
							{
								binaryContent.Headers.ContentType = new("application/octet-stream");
								multipart.Add(binaryContent, Path.GetFileName(filepath), Path.GetFileName(filepath));
								var response = await client.PostAsync("http://localhost:5153/api/add", multipart);
								Console.WriteLine($"Response {filepath}:");
								if (response.IsSuccessStatusCode)
								{
									Console.WriteLine($"Uploaded successfully.");
								}
								else
								{
									Console.WriteLine($"Error uploading: {response.ReasonPhrase}");
								}
							}
						}
					}

					// Request processing of urls. Server retrieves the content.
					var urls = cli.GetUrls();
					if (urls.Count() > 0)
					{
						using var client2 = new HttpClient();
						var response = await client2.PostAsJsonAsync("http://localhost:5153/api/add", new DocumentAddRequest() { Tags = cli.GetTags(), Urls = urls }, ConfigExtensions.JsonOptions);
						if (response.IsSuccessStatusCode)
						{
							Console.WriteLine($"Uploaded successfully.");
						}
						else
						{
							Console.WriteLine($"Error uploading: {response.ReasonPhrase}");
						}
					}
					break;

				case CommandLineStartupArguments.CommandArg.None:
				default:
					Console.WriteLine("No command specified. Use 'help' for more information.");
					break;
			}
		}
	}
}
