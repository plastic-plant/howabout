using Howabout.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Howabout.Extensions.FilePathExtension;

namespace Howabout.Tests.Extensions
{
	public class FilePathExtensionsTests
	{
		[Fact]
		public void ToFullPaths_Returns()
		{
			// Arrange
			var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "Examples", ".gitkeep");
			var relative = Path.Combine("Examples", ".gitkeep");
			var expected = new string[] { fullpath };
			var given = new string[] { relative };

			// Act
			var actual = given.ToFullPaths(RelativePathMatchingStrategy.TryCurrentWorkingDirectory, failIfNotFound: true);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ToFullPaths_FailIfNotFound_Throws()
		{
			var given = new string[] { "unknown_file.txt" };

			var actual = () => given.ToFullPaths(RelativePathMatchingStrategy.TryCurrentWorkingDirectory, failIfNotFound: true);

			Assert.Throws<ArgumentException>(actual);
		}


		[Theory]
		[InlineData(RelativePathMatchingStrategy.TryAll, false, "/path/to/unknown_file.txt", "/path/to/unknown_file.txt")]
		[InlineData(RelativePathMatchingStrategy.TryAll, false, "relative_path/to/unknown_file.txt", "relative_path/to/unknown_file.txt")]
		[InlineData(RelativePathMatchingStrategy.TryAll, false, ".\\relative_path\\to\\unknown_file.txt", ".\\relative_path\\to\\unknown_file.txt")]
		[InlineData(RelativePathMatchingStrategy.TryAll, false, "C:\\path\\to\\unknown_file.txt", "C:\\path\\to\\unknown_file.txt")]
		[InlineData(RelativePathMatchingStrategy.TryAll, false, "unknown_file.txt", "unknown_file.txt")]
		[InlineData(RelativePathMatchingStrategy.TryAll, true, "https://en.wikipedia.org/wiki/Lisa_Simpson", "https://en.wikipedia.org/wiki/Lisa_Simpson")]
		public void ToFullPaths_ContinueIfNotFound_ReturnsUnknownPath(RelativePathMatchingStrategy options, bool failIfNotFound, string pathIn, string pathOut)
		{
			var expected = new string[] { pathOut };
			var given = new string[] { pathIn };

			var actual = given.ToFullPaths(options, failIfNotFound);

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void IncludeDirectoryFiles_WithExistingFile_Returns()
		{
			// Arrange
			var expected = Path.Combine(Directory.GetCurrentDirectory(), "Examples", ".gitkeep");
			var directory = Path.GetDirectoryName(expected)!; 			
			var given = new string[] { directory };

			// Act
			var actual = given.IncludeDirectoryFiles();

			// Assert
			Assert.Contains(expected, actual);
		}
	}
}
