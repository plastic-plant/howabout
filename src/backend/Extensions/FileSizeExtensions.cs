using System.Globalization;

namespace Howabout.Extensions
{
	public static class FileSizeExtensions
	{
		private static string[] Units =  { "B", "KB", "MB", "GB", "TB" };

		public static string ToFileSizeFormatted(this long bytesLength, int precision = 0)
		{
            if (bytesLength == 0)
            {
				return string.Empty;
            }
            double pow = Math.Floor((bytesLength > 0 ? Math.Log(bytesLength) : 0) / Math.Log(1024));
			pow = Math.Min(pow, Units.Length - 1);
			double value = (double)bytesLength / Math.Pow(1024, pow);
			return value.ToString(pow == 0 ? "F0" : "F" + precision.ToString(), CultureInfo.InvariantCulture) + " " + Units[(int)pow];
		}
	}
}