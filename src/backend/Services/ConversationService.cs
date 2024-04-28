using Howabout.Hubs;
using Howabout.Interfaces;
using Howabout.Models;
using Microsoft.AspNetCore.SignalR;

namespace Howabout.Services
{
	public class ConversationService : IConversationService
	{
		private readonly IHubContext<EventMessageHub, IEventMessageClient> _eventMessageHub;
		private readonly IKernelMemoryService _kernelMemoryService;
		private readonly List<ConversationMessage> _messages = new();

		public ConversationService(IHubContext<EventMessageHub, IEventMessageClient> eventMessageHub, IKernelMemoryService kernelMemoryService)
        {
            _eventMessageHub = eventMessageHub ?? throw new ArgumentNullException(nameof(eventMessageHub));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));

		}

        public void AddMessage(ConversationMessage message)
		{
			_eventMessageHub.Clients.All.MessageAddedEvent(message);
			_messages.Add(message);
		}

		public async Task<List<ConversationMessage>> GetMessagesAsync()
		{
			// bool isReadyWithFirstUpload = _messages.Count < 3 && _messages.First().MessageType == ConversationMessageType.DocumentChange;
			bool isEmptyNeedsWelcome = _messages.Count == 0;
			if (isEmptyNeedsWelcome)
			{
				bool isReady = await _kernelMemoryService.IsReadyAsync();
				_messages.Add(new ConversationMessage
				{
					MessageType = isReady ? ConversationMessageType.WelcomeUploadRequired : ConversationMessageType.WelcomeSetupRequired,
					Role = ConversationMessageRole.Assistant,
					MessageText = isReady ? "Welcome! Howabout started succesfully. Please upload a document to get started. You can then ask questions about it. If you like to, you can add tags to group multiple documents." : "Welcome! Howabout started succesfully. I see you haven't setup providers for embedding and conversation in my appsettings.json configuration. Can I help you setup?"
				});
			}
			return _messages;
		}
	}
}
