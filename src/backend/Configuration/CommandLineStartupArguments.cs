namespace Howabout.Configuration
{
	/// <summary>
	/// howabout.exe <command> <option>  [flag] [parameter]
	/// howabout.exe add       file.docx -v     --tag archive2024
	/// 
	/// Command     is the first argument and is required, e.g. 'add', 'start', 'help'.
	/// Arguments   holds a copy of all command-line arguments, except the first command argument.
	/// Options     are arguments that are not -flags or --parameters keyvalue, e.g. 'file.txt'.
	/// Parameters  are arguments that start with a double dash and have a value, e.g. '--tag archive2024'.
	/// Flags       are arguments that start with a single or double dash and have no value, e.g. '-v' and '--verbose'.
	/// </summary>
	public class CommandLineStartupArguments
	{
        public CommandArg Command { get; set; } = CommandArg.None;
		public List<string> Arguments { get; set; } = new();
		public List<string> Options { get; set; } = new();
		public List<string> Flags { get; set; } = new();		
		public List<KeyValuePair<string, string>> Parameters { get; set; } = new();

		public CommandLineStartupArguments(string[] args)
		{			
			ParseStartupArguments(args);
		}
		public enum CommandArg
		{
			None,
			Help,
			Version,
			Start,
			Stop,
			Add,
			Ask,
		}

		void ParseStartupArguments(string[] args)
		{
			if (args.Length > 0)
			{
				Command = args[0]?.ToLowerInvariant() switch
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
					"ask" => CommandArg.Ask,

					_ => CommandArg.None
				};

			}

			if (args.Length > 1)
			{
				Arguments = args.Skip(1).ToList();
				var argumentsLength = Arguments.Count();
				var skipNext = false;
				for (int i = 0; i < argumentsLength; i++)
				{
					if (skipNext)
					{
						// The argument is a parameter value already added in last loop, e.g. --tag tagvalue.
						skipNext = false;
						continue;
					}

					var thisArgument = Arguments[i];
					var thisArgumentIsParameter = thisArgument.StartsWith("--");
					var thisArgumentIsFlag = !thisArgumentIsParameter && thisArgument.StartsWith("-");
					var nextArgument = i + 1 < argumentsLength ? Arguments[i + 1] : null;

					if (thisArgumentIsFlag)
					{
						// -v
						Flags.Add(thisArgument);
					}
					else if (thisArgumentIsParameter && nextArgument is null)
					{
						// --verbose
						Flags.Add(thisArgument);

					}
					else if (thisArgumentIsParameter && nextArgument is not null)
					{
						// --tag tagvalue
						Parameters.Add(new(thisArgument, nextArgument));
						skipNext = true;
					}
					else
					{
						// file.txt
						Options.Add(thisArgument);
					}
				}
			}
		}

		public List<string> GetTags() => Parameters.ToLookup(kvp => kvp.Key, kvp => kvp.Value)["--tag"].ToList();
		public List<string> GetUrls() => Options.Where(option => option.Contains("://") && Uri.TryCreate(option, UriKind.Absolute, out _)).ToList();
	}
}
