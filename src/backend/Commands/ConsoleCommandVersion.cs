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
			Log.Information("{Version}", typeof(Program).Assembly.GetName()?.Version?.ToString(2));
			return Task.CompletedTask;
		}

	}
}
