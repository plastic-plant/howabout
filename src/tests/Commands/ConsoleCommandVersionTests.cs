using FluentAssertions;
using Howabout.Commands;
using Howabout.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandVersionTests
	{
		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "version" });
			var sut = new ConsoleCommandVersion(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}

		[Fact]
		public async Task Execute_Succeeds()
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			using (TestCorrelator.CreateContext())
			{
				var givenArgs = new ConsoleStartupArguments(new string[] { "version" });
				var sut = new ConsoleCommandVersion(givenArgs);

				await sut.Execute();

				string matchVersionNumber = @"\d+\.\d+";
				var logevents = TestCorrelator.GetLogEventsFromCurrentContext();

				TestCorrelator.GetLogEventsFromCurrentContext()
					.Should().ContainSingle()
					.Which.Properties.Should().ContainKey("Version")
						.WhoseValue.Should().BeOfType<ScalarValue>()
						.Which.Value.Should().BeOfType<string>()
						.Which.Should().MatchRegex(matchVersionNumber);
			}
		}
	}
}
