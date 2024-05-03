
export async function getMessages(): Promise<IConversationMessage[]> {
    try {
        const response = await fetch('/api/messages');

        if (response.ok) {
            const data: IConversationMessage[] = await response.json();
            return data;
        } else {
            return [];
        }
    } catch (error) {
        return [];
    }
};

export async function askMessage(message: string): Promise<string> {
    try {
        const response = await fetch('/api/ask', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ question: message }),

        });

        if (response.ok) {
            await response.json();
            return '';
        } else {
            return 'Error adding message.';
        }
    } catch (error) {
        return 'Error adding message.';
    }
};
