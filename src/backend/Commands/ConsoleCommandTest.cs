using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandTest : IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;

        public ConsoleCommandTest(ConsoleStartupArguments args)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));            
        }

        public Task Verify()
		{
			Log.Information("Test.Verify");
			if (_args.Options.Contains("throw-error"))
			{
				throw new ArgumentException("Test by throwing an exception.");
			}
			return Task.CompletedTask;
		}

		public Task Execute()
		{
			Log.Information("Test.Execute");
			return Task.CompletedTask;
		}

	}
}
