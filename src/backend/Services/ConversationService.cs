using Howabout.Hubs;
using Howabout.Interfaces;
using Howabout.Models;
using Microsoft.AspNetCore.SignalR;

namespace Howabout.Services
{
	public class ConversationService : IConversationService
	{
		private readonly IHubContext<EventMessageHub, IEventMessageClient> _eventMessageHub;
		private readonly List<ConversationMessage> _messages = new();

        public ConversationService(IHubContext<EventMessageHub, IEventMessageClient> eventMessageHub)
        {
            _eventMessageHub = eventMessageHub ?? throw new ArgumentNullException(nameof(eventMessageHub));
        }

        public void AddMessage(ConversationMessage message)
		{
			_eventMessageHub.Clients.All.MessageAddedEvent(message);
			_messages.Add(message);
		}

		public List<ConversationMessage> GetMessages()
		{
			if (_messages.Count == 0)
			{
				_messages.Add(new ConversationMessage
				{
					MessageType = ConversationMessageType.WelcomeReady,
					Role = ConversationMessageRole.Assistant,
					MessageText = "Welcome!",
					Time = DateTime.UtcNow,
				});
			}
			return _messages;
		}
	}
}
