using Howabout.Models;

namespace Howabout.Interfaces
{
	public interface IConversationService
	{
		Task<List<ConversationMessage>> GetMessagesAsync();
		void AddMessage(ConversationMessage message);
	}
}
