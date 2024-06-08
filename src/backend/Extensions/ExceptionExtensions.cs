namespace Howabout.Extensions
{
	public static class ExceptionExtensions
	{
		public static string GetFullMessage(this Exception exception)
		{
			var message = exception.Message;
			var innerException = exception.InnerException;
			while (innerException != null)
			{
				message += " " + innerException.Message;
				innerException = innerException.InnerException;
			}
			return message;
		}
	}
}
