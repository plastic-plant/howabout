
import { useEffect, useState } from 'react';
import EventMessageService from './EventMessageService';
function Conversation() {
    const { events } = EventMessageService();
    useEffect(() => {
        const handleDocumentAddedEvent = (message: string) => setMessages([...messages, message]);
        const handleDocumenRemovedEvent = (message: string) => setMessages(messages.filter((value) => value !== message));
        events(handleDocumentAddedEvent, handleDocumenRemovedEvent);
    });

    const [messages, setMessages] = useState<string[]>([]);

    return (
        <div>
        <h3>Conversation</h3>
            {messages.map((message, index) => (
                <article key={index}>{message}</article>
            ))}
        </div>
    );
}

export default Conversation;
