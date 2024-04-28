namespace Howabout.Models
{
	public class ConversationMessage
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
        public ConversationMessageType MessageType { get; set; } = ConversationMessageType.None;
		public ConversationMessageRole Role { get; set; } = ConversationMessageRole.None;
		public DateTime Time { get; set; } = DateTime.UtcNow;
		public string MessageText { get; set; } = string.Empty;
	}
}
