using FluentAssertions;
using Howabout.Models;
using Serilog;
using Serilog.Sinks.TestCorrelator;

namespace Howabout.Tests
{
    public class ProgramTests
	{

		[Fact]
		public async Task Main_WithCommand_ShouldVerifyAndExecute()
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			using (TestCorrelator.CreateContext())
			{
				var exitcode = await Program.Main(new string[] { "test" });

				TestCorrelator.GetLogEventsFromCurrentContext()
					.Should().HaveCount(2)
					.And.Subject.Select(logevent => logevent.MessageTemplate.Text)
					.Should().Contain("Test.Verify")
					.And.Contain("Test.Execute");

				exitcode.Should().Be((int)ConsoleExit.Success);
			}
		}

		[Fact]
		public async Task Main_WithCommand_ShouldExitOnError()
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			using (TestCorrelator.CreateContext())
			{
				TestCorrelator.GetLogEventStreamFromCurrentContext()
					.Subscribe(logEvent => throw new Exception());

				var exitcode = await Program.Main(new string[] { "test", "throw-error"});
				exitcode.Should().Be((int)ConsoleExit.Error);
			}
		}
	}
}
