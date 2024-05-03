interface IConversationMessage {
	id: string;
	messageType: string;
	role: string;
	time: string;
	messageText: string;
	messageData: any;
	processingTimeSeconds: number
}