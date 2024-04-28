
import { useEffect, useState } from 'react';
import EventMessageService from './EventMessageService';
import { getMessages } from './ConversationMessageService';

function Conversation() {
    const [messages, setMessages] = useState<ConversationMessage[]>([]);
    const { events } = EventMessageService();

    useEffect(() => {
        const loadMessages = async () => {
            const messages = await getMessages();
            setMessages(messages);
        };
        loadMessages();

        const handleMessageAddedEvent = (message: ConversationMessage) => setMessages([...messages, message]);
        events(undefined, handleMessageAddedEvent);
    }, []);

    return (
        <div>
            {messages ? messages.map((message) => (
                <article key={message.id}>{message.messageText}</article>
            )) : (
            <p>Loading...</p>
            )}
        </div>
    );
}

export default Conversation;

