using FluentAssertions;
using Howabout.Commands;
using Howabout.Configuration;
using Moq;
using Moq.Protected;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using System.Net;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandAskTests
	{
		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "ask" });
			var sut = new ConsoleCommandAdd(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}

		[Theory]
		[InlineData("ask", "This is a first question?")]
		[InlineData("ask", "This", "is", "a", "second", "question?")]
		[InlineData("ask", "'This is a third question in single quotes?'")]
		[InlineData("ask", """This is a fourth question in double quotes?""")]
		public async Task Executes_Succeeds(params string[] givenQuestion)
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			using (TestCorrelator.CreateContext())
			{
				var givenArgs = new ConsoleStartupArguments(givenQuestion);
				var expectedAnswer = "Here's a summary";
				var httpMessagehandler = new Mock<HttpClientHandler>();
				httpMessagehandler
					.Protected()
					.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
					.ReturnsAsync(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(expectedAnswer)
					});
				var sut = new ConsoleCommandAsk(givenArgs, httpMessagehandler.Object);

				await sut.Execute();

				TestCorrelator.GetLogEventsFromCurrentContext()
					.Should().ContainSingle()
					.And.Subject.Select(logevent => logevent.MessageTemplate.Text)
					.Should().Contain(expectedAnswer);
			}
		}
	}
}
