using Howabout.Configuration;
using Howabout.Repositories;

namespace Howabout.Tests.Repositories
{
	public class DownloadRepositoryTests
	{
		[Fact]
		public void DownloadRepository_SetsCredentials()
		{
			// Arrange
			var args = new ConsoleStartupArguments(new[] { "download", "repo/model", "--username", "sixfingers", "--password", "123456" });
			
			// Act
			var downloadRepository = new DownloadRepository(args);

			// Assert
			Assert.True(downloadRepository.IsGivenCredentials);
		}
	}
}
