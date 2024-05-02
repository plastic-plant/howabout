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

namespace Howabout.Tests.Commands
{
	public class ConsoleCommandStartTests
	{
		[Fact]
		public void Verify_Succeeds()
		{
			var givenArgs = new ConsoleStartupArguments(new string[] { "start" });
			var sut = new ConsoleCommandStart(givenArgs);

			var actual = sut.Verify();

			Assert.Equal(Task.CompletedTask, actual);
		}
	}
}
