using Howabout.Commands;
using Howabout.Configuration;
using Xunit.Abstractions;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandHelpTests
	{
		private readonly ITestOutputHelper _consoleOutput;

		public ConsoleCommandHelpTests(ITestOutputHelper output)
		{
			_consoleOutput = output;
		}

		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "help" });
			var sut = new ConsoleCommandHelp(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}

		[Fact]
		public async Task Execute_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "help" });
			var sut = new ConsoleCommandHelp(givenArgs);

			await sut.Execute();

			Assert.Contains("Usage: howabout <command> [options]", _consoleOutput.ToString());
		}
	}
}
