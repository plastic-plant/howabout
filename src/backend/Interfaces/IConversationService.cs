using Howabout.Models;

namespace Howabout.Interfaces
{
	public interface IConversationService
	{
		bool HasMessages();
		List<ConversationMessage> GetMessagesAsync();
		void AddMessage(ConversationMessage message);
	}
}
