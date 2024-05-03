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
			if (message.MessageText.Contains("INFO NOT FOUND"))
			{
				message.MessageText = "Sorry, I couldn't find any information on that in the uploaded documents.";
			}
			_messages.Add(message);
			_eventMessageHub.Clients.All.MessageAddedEvent(message);
		}

		public async Task<List<ConversationMessage>> GetMessagesAsync()
		{
			await IncludeStartupMessages();

			return _messages;
		}

		/// <summary>
		/// Flow control. Should not be here, just a quick inject as proof of concept.
		/// </summary>
		private async Task IncludeStartupMessages()
		{
			bool isEmptyNeedsWelcome = _messages.Count == 0;
			if (isEmptyNeedsWelcome)
			{
				bool isReady = await _kernelMemoryService.IsReadyAsync();
				_messages.Add(new ConversationMessage
				{
					MessageType = isReady ? ConversationMessageType.WelcomeUploadRequired : ConversationMessageType.WelcomeSetupRequired,
					Role = ConversationMessageRole.Assistant,
					MessageText = isReady ? "Welcome! Please upload a document to get started. You can then ask questions about it." : "Welcome! It sems you haven't setup providers for embedding and conversation. Open appsettings.json configuration and start again."
				});
			}

			bool isReadyWithFirstUpload = _messages.Count < 3 && _messages.First().MessageType == ConversationMessageType.DocumentChange;
			if (isReadyWithFirstUpload)
			{
				bool isReady = await _kernelMemoryService.IsReadyAsync();
				_messages.Add(new ConversationMessage
				{
					MessageType = ConversationMessageType.WelcomeReady,
					Role = ConversationMessageRole.Assistant,
					MessageText = "Nice, we have a document upload. How about you ask for a summary?"
				});
			}
		}
	}
}
