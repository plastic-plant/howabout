namespace Howabout.Models
{
	public class ConversationMessage
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
        public ConversationMessageType MessageType { get; set; } = ConversationMessageType.None;
		public ConversationMessageRole Role { get; set; } = ConversationMessageRole.None;
		public string Time { get; set; } = DateTime.UtcNow.ToString("HH:mm");
		public string MessageText { get; set; } = string.Empty;

        public Object? MessageData { get; set; } // DocumentProperties, List<Citation>
        public int ProcessingTimeSeconds { get; set; }
    }
}
