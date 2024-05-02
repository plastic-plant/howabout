using Howabout.Configuration;
using Howabout.Interfaces;

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
			Console.WriteLine("Help information.");
			return Task.CompletedTask;
		}

	}
}
