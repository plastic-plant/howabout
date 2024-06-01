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
			_messages.Add(new ConversationMessage
			{
				MessageType = ConversationMessageType.WelcomeUploadRequired,
				MessageText = "Welcome! Please upload a document to get started. You can then ask questions about it.",
				Role = ConversationMessageRole.Assistant,
			});
		}

		public bool HasMessages() => _messages.Count > 0;

		public void AddMessage(ConversationMessage message)
		{
			if (message.MessageText.Contains("INFO NOT FOUND"))
			{
				message.MessageText = "Sorry, I couldn't find any information on that in the uploaded documents.";
			}
			_messages.Add(message);
			_eventMessageHub.Clients.All.MessageAddedEvent(message);
		}

		public List<ConversationMessage> GetMessagesAsync()
		{
			return _messages;
		}
	}
}
