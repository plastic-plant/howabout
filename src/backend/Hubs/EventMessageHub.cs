using Microsoft.AspNetCore.SignalR;

namespace Howabout.Hubs
{
	public class EventMessageHub: Hub<IEventMessageClient>
	{
		public async Task DocumentAddedEvent(string message)
			=> await Clients.All.DocumentAddedEvent(message);
		public async Task DocumentRemoveddEvent(string message)
			=> await Clients.All.DocumentRemovedEvent(message);

	}

	public interface IEventMessageClient
	{
		Task DocumentAddedEvent(string message);
		Task DocumentRemovedEvent(string message);
	}
}
