namespace Howabout.Models
{
	public enum ConversationMessageType
	{
		None,
		WelcomeSetupRequired, // with the option to help stop and setup the server settings.
		WelcomeUploadRequired, // with the option to have a quick example test upload.
		WelcomeReady, // with an invitation to start a conversation.
		DocumentChange, // with a document icon and file details.
		Conversation
	}
}
