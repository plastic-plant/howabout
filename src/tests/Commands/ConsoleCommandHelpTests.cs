using FluentAssertions;
using Howabout.Commands;
using Howabout.Configuration;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Xunit.Abstractions;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandHelpTests
	{
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
			using (TestCorrelator.CreateContext())
			{
				using var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger();
				Log.Logger = logger;

				var givenArgs = new ConsoleStartupArguments(new string[] { "help" });
				var sut = new ConsoleCommandHelp(givenArgs);

				await sut.Execute();

				var logs = TestCorrelator.GetLogEventsFromCurrentContext();
				logs.Should().ContainSingle()
				.And.Subject.Select(logevent => logevent.MessageTemplate.Text)
				.Should().Contain(entry => entry.Contains("Usage: howabout <command> [options]"));
			}
		}
	}
}
