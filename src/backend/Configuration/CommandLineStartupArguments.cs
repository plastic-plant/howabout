namespace Howabout.Configuration
{
	public class CommandLineStartupArguments
	{
		private readonly string[] _startupArguments;

        public CommandArg Command { get; set; } = CommandArg.None;

        public CommandLineStartupArguments(string[] args)
		{
			_startupArguments = args;
			ParseStartupArguments();
		}

		void ParseStartupArguments()
		{
			var command = _startupArguments.Length > 0 ? _startupArguments[0].ToLowerInvariant() : "";
			Command = command switch
			{
				"/?" => CommandArg.Help,
				"-h" => CommandArg.Help,
				"--help" => CommandArg.Help,
				"help" => CommandArg.Help,

				"-v" => CommandArg.Version,
				"--version" => CommandArg.Version,
				"version" => CommandArg.Version,

				"start" => CommandArg.Start,
				"stop" => CommandArg.Stop,
				"add" => CommandArg.Add,

				_ => CommandArg.None
			};
		}

		public enum CommandArg
		{
			None,
			Help,
			Version,
			Start,
			Stop,
			Add,
		}
	}
}
