interface ConversationMessage {
	id: string;
	messageType: string;
	role: string;
	time: string;
	messageText: string;
	messageData: any;
}