using Howabout.Commands;
using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;
using Serilog.Events;
using static Howabout.Configuration.ConsoleStartupArguments;

namespace Howabout
{
	public class Program
	{
		public static async Task<int> Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} {Message:lj}{NewLine}{Exception}")
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.KernelMemory.Handlers", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.CreateLogger();

			//Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

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
