namespace Howabout.Configuration
{
	public enum SignalRProtocol
	{
		Http1,
		Http2,
		WebSockets
	}

	public static class SignalRProtocolHelpers
	{

		public static List<string> LongPollingOnly()
		{
			return new List<string> { SignalRProtocol.Http1.ToString(), SignalRProtocol.Http2.ToString() };
		}
	}
	
}
