using Howabout.Models;
using Microsoft.AspNetCore.SignalR;

namespace Howabout.Hubs
{
	public class EventMessageHub: Hub<IEventMessageClient>
	{
		public async Task DocumentChangedEvent()
			=> await Clients.All.DocumentChangedEvent();
		public async Task MessageAddedEvent(ConversationMessage message)
			=> await Clients.All.MessageAddedEvent(message);
	}

	public interface IEventMessageClient
	{
		Task DocumentChangedEvent();
		Task MessageAddedEvent(ConversationMessage message);
	}
}
