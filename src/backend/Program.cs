using Howabout.Commands;
using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;
using System.Reflection;
using static Howabout.Configuration.ConsoleStartupArguments;

namespace Howabout
{
	public class Program
	{
		public static string OriginalWorkingDirectory = Directory.GetCurrentDirectory();
		public static string ApplicationRootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? OriginalWorkingDirectory;

		public static async Task<int> Main(string[] args)
		{
			if (Log.Logger.IsDefaultUnconfigured())
			{
				Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(App.Settings.Configuration)
					.Enrich.FromLogContext()
					.CreateLogger();
			}

			App.Settings = new AppSettings()
				.ReadFromAppSettings()
				.ReadFromEnvironmentVariables()
				.VerifyOrThrow();

			var cli = new ConsoleStartupArguments(args);
			IConsoleCommand command = cli.Command switch
			{
				CommandArg.Help => new ConsoleCommandHelp(cli),
				CommandArg.Version => new ConsoleCommandVersion(cli),
				CommandArg.Start => new ConsoleCommandStart(cli),
				CommandArg.Stop => new ConsoleCommandStop(cli),
				CommandArg.Add => new ConsoleCommandAdd(cli),
				CommandArg.Ask => new ConsoleCommandAsk(cli),
				CommandArg.Test => new ConsoleCommandTest(cli),
				CommandArg.None => new ConsoleCommandDefault(cli),
				_ => new ConsoleCommandDefault(cli),
			};
			try
			{
				await command.Verify();
				await command.Execute();
				return (int)ConsoleExit.Success;
			}
			catch (Exception exception)
			{
				Log.Error(exception.Message);
				Log.Verbose(exception, "Error details:");
				return (int)ConsoleExit.Error;
			}
		}
	}
}
