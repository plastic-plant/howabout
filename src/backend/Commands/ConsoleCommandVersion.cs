using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandVersion: IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;

        public ConsoleCommandVersion(ConsoleStartupArguments args)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));            
        }

        public Task Verify()
		{
			return Task.CompletedTask;
		}

		public Task Execute()
		{
			var os = Environment.OSVersion;
			Log.Information("{AppVersion}|{PlatformSystem:G}|{PlatformVersion}", typeof(Program).Assembly.GetName()?.Version?.ToString(2), os.Platform, os.VersionString);
			return Task.CompletedTask;
		}

	}
}
