using Howabout.Commands;
using Howabout.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.SemanticKernel;
using Moq.Protected;
using System.Net;
using FluentAssertions;

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandAddTests
	{
		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "add" });
			var sut = new ConsoleCommandAdd(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}

		[Theory]
		[InlineData("Examples/small_snips", 4)]
		[InlineData("Examples\\small_snips", 4)]
		[InlineData("Examples/small_snips/today.txt", 1)]
		[InlineData("http://anyserver/testdocument.pdf", 1)]
		public async Task Execute_Succeeds(string givenPath, int expectedCount)
		{
			Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
			App.Settings = new AppSettings { Url = "http://anyhost/" };
			using (TestCorrelator.CreateContext())
			{
				var givenArgs = new ConsoleStartupArguments(new string[] { "add", givenPath });
				var httpMessagehandler = new Mock<HttpClientHandler>();
				httpMessagehandler
					.Protected()
					.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
					.ReturnsAsync(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(string.Empty)
					});
				var sut = new ConsoleCommandAdd(givenArgs, httpMessagehandler.Object);

				await sut.Execute();

				TestCorrelator.GetLogEventsFromCurrentContext()
					.Should().HaveCount(expectedCount)
					.And.Subject.Select(logevent => logevent.MessageTemplate.Text)
					.Should().Contain(entry => entry.Contains("Document added: {FileName}") || entry.Contains("Urls added: {Url}"));
			}
		}
	}
}
