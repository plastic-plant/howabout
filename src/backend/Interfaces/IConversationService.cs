using Howabout.Models;

namespace Howabout.Interfaces
{
	public interface IConversationService
	{
		List<ConversationMessage> GetMessages();
		void AddMessage(ConversationMessage message);
	}
}
