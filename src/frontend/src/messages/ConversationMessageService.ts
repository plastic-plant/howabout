
export async function getMessages(): Promise<ConversationMessage[]> {
    try {
        const response = await fetch('/api/messages');

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            return [];
        }
    } catch (error) {
        return [];
    }
};

export async function addMessage(message: ConversationMessage): Promise<string> {
    try {
        const response = await fetch('/api/messages/add', {
            method: 'POST',
            body: JSON.stringify(message),
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            return 'Error adding message.';
        }
    } catch (error) {
        return 'Error adding message.';
    }
};
