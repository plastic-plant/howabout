using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandDefault : IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;

        public ConsoleCommandDefault(ConsoleStartupArguments args)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));            
        }

        public Task Verify()
		{
			return Task.CompletedTask;
		}

		public Task Execute()
		{
			Log.Information(@"
Usage: howabout <command> [options]

Commands:
  help      Display help information.
  download  Download models.
  start     Start the server.
  stop      Stop the server.
  add       Add documents to the server.
  ask       Ask questions.

Type 'howabout help' for examples.
Type 'howabout start' to run server.
");
			return Task.CompletedTask;
		}

	}
}
