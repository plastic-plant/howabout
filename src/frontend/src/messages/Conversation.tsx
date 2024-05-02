
import React, { useEffect, useRef, useState } from 'react';
import EventMessageService from './EventMessageService';
import { getMessages } from './ConversationMessageService';
import WelcomeHelpMessage from './WelcomeHelpMessage';
import DocumentUploadedMessage from './DocumentUploadedMessage';


export default function Conversation() {
    const [messages, setMessages] = useState<ConversationMessage[]>([]);
    const { events } = EventMessageService();

    let mounted = false;

    const messagesEndRef = useRef<null | HTMLDivElement>(null)
    const handleMessageAddedEvent = (message: ConversationMessage) => {
        setMessages(prevMessages => [...prevMessages, message]);
        scrollToLastmessage(messagesEndRef);
    }
    const loadMessages = async () => setMessages(await getMessages());
    
    useEffect(() => {
        if (!mounted) { // Prevents multiple event subscriptions when running in dev-mode with React.Strict.
            loadMessages();
            scrollToLastmessage(messagesEndRef);
            events(undefined, handleMessageAddedEvent);
        }
        mounted = true;
    }, []);

    return (
        <section>
            {messages ? messages.map((message: ConversationMessage) => (
                <div key={message.id}>
                    <ConversationMessageComponent message={message} />
                </div>
            )) : (
            <p>Loading...</p>
            )}
            <div ref={messagesEndRef} />
        </section>
    );
}

function scrollToLastmessage(messagesEndRef: React.MutableRefObject<HTMLDivElement | null>) {
    window.setTimeout(() => messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' }), 0);
}
function ConversationMessageComponent({ message }: { message: ConversationMessage }) {

    const roleProfileImage = `/${message.role}.jpg`;
    switch (message.messageType) {

        case 'welcomeSetupRequired': // with the option to help stop and setup the server settings.
        case 'welcomeUploadRequired': // with the option to have a quick example test upload.
        case 'welcomeReady': // with an invitation to start a conversation.
            return <WelcomeHelpMessage message={message} />;

        case 'documentChange': // with a document icon and file details.
            return <DocumentUploadedMessage message={message} />;

        case 'conversation': // for regular request and response messages with model.
        case 'None':
        default:
            return (
                <article key={message.id} className="flex items-start gap-2.5 my-6">
                    <img className="w-8 h-8 rounded-full" src={roleProfileImage} />
                    <div className="flex flex-col gap-1 w-full max-w-[320px]">
                        <div className="flex items-center space-x-2 rtl:space-x-reverse">
                            <span className="text-sm font-semibold text-gray-900 dark:text-white capitalize">{message.role}</span>
                            <span className="text-sm font-normal text-gray-500 dark:text-gray-400">{message.time}</span>
                        </div>
                        <div className="flex flex-col leading-1.5 p-4 border-gray-200 bg-gray-100 rounded-e-xl rounded-es-xl dark:bg-gray-700">
                            <p className="text-sm font-normal text-gray-900 dark:text-white">{message.messageText}</p>
                        </div>
                        <span className="text-sm font-normal text-gray-500 dark:text-gray-400">Ready</span>
                    </div>
                </article>
            );
    }
}
