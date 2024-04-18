using Howabout.Configuration;
using static Howabout.Configuration.CommandLineStartupArguments;

namespace Howabout.Tests.Configuration
{
	public class CommandLineStartupArgumentsTests
	{
		[Theory]
		[InlineData("/?", CommandArg.Help)]
		[InlineData("-h", CommandArg.Help)]
		[InlineData("--help", CommandArg.Help)]
		[InlineData("help", CommandArg.Help)]
		[InlineData("-v", CommandArg.Version)]
		[InlineData("--version", CommandArg.Version)]
		[InlineData("version", CommandArg.Version)]
		[InlineData("start", CommandArg.Start)]
		[InlineData("stop", CommandArg.Stop)]
		[InlineData("add", CommandArg.Add)]
		[InlineData("unknown_argument", CommandArg.None)]
		[InlineData("", CommandArg.None)]
		[InlineData(null, CommandArg.None)]
		public void ParseStartupArguments_Returns(string given, CommandArg expected)
		{
			var args = new string[] { given };
			var sut = new CommandLineStartupArguments(args);
			Assert.Equal(expected, sut.Command);
		}
	}
}
