
using Howabout.Configuration;
using Howabout.Interfaces;
using Howabout.Services;

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
					builder.Services.AddSingleton<IKernelMemoryService, KernelMemoryService>();
					builder.Services.AddControllers();
					builder.Services.AddEndpointsApiExplorer();
					builder.Services.AddSwaggerGen();

					var app = builder.Build();
					app.UseDefaultFiles();
					app.UseStaticFiles();
					app.UseSwagger();
					app.UseSwaggerUI();
					app.MapControllers();
					app.MapFallbackToFile("/index.html");
					await app.RunAsync();
					break;

				case CommandLineStartupArguments.CommandArg.Stop:
					Console.WriteLine("Stop command.");
					break;

				case CommandLineStartupArguments.CommandArg.Add:
					Console.WriteLine("Add command.");
					break;

				case CommandLineStartupArguments.CommandArg.None:
				default:
					Console.WriteLine("No command specified. Use 'help' for more information.");
					break;
			}
		}
	}
}
