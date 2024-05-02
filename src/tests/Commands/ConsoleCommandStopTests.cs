using Howabout.Commands;
using Howabout.Configuration;
using Moq.Protected;
using Moq;
using Serilog.Sinks.TestCorrelator;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandStopTests
	{
		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "ask" });
			var sut = new ConsoleCommandAdd(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}

		[Fact]
		public async Task Executes_Succeeds()
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			using (TestCorrelator.CreateContext())
			{
				var givenArgs = new ConsoleStartupArguments(new string[] { "stop" });
				var expectedAnswer = "Stopped.";
				var httpMessagehandler = new Mock<HttpClientHandler>();
				httpMessagehandler
					.Protected()
					.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
					.ReturnsAsync(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(expectedAnswer)
					});
				var sut = new ConsoleCommandStop(givenArgs, httpMessagehandler.Object);

				await sut.Execute();

				TestCorrelator.GetLogEventsFromCurrentContext()
					.Should().ContainSingle()
					.And.Subject.Select(logevent => logevent.MessageTemplate.Text)
					.Should().Contain(expectedAnswer);
			}
		}
	}
}
