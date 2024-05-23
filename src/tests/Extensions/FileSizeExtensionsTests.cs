using Howabout.Extensions;

namespace Howabout.Tests.Extensions
{
	public class FileSizeExtensionsTests
	{
		[Theory]
		[InlineData(0, 0, "")]
		[InlineData(128, 0, "128 B")]
		[InlineData(128, 2, "128 B")]
		[InlineData(1024, 0, "1 KB")]
		[InlineData(10240, 2, "10.00 KB")]
		[InlineData(102400, 2, "100.00 KB")]
		[InlineData(1048576, 1, "1.0 MB")]
		[InlineData(1048576, 2, "1.00 MB")]
		public void CanFormatFileSizes(long givenBytesLength, int givenPrecision, string expectedFormatted)
		{
			Assert.Equal(expectedFormatted, givenBytesLength.ToFileSizeFormatted(givenPrecision));
		}
	}
}
