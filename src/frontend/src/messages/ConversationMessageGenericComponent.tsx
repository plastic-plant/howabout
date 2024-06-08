import React, { useRef } from 'react';
import { ConversationMessageRole, ICitation, IConversationMessage } from './Models';

const ConversationMessageGenericComponent: React.FC<{ message: IConversationMessage }> = ({ message }) => {
    const citationsRef = useRef<null | HTMLDivElement>(null);
    const roleProfileImage = `/${message.role}.png`;
    const backgroundColour = message.role === 'user' ? 'bg-lightorange' : 'bg-gray-100';
    const citations: string[] = message.messageData && Array.isArray(message.messageData) && message.messageData.flatMap((citation: { partitions: any[]; }) => citation.partitions.map(partition => partition.text)) || [];

    const stopServer = async () => {
        const response = await fetch('/api/stop');
        if (response.ok) {
            window.alert("Couldn't stop the server.");
        } else {
            window.alert('Stopped. Bye.');
        }
    };

    return (
        <article key={message.id} className="flex items-start gap-2.5 my-6">
            <img className="w-8 h-8 rounded-full" src={roleProfileImage} alt={`${message.role} profile picture`} />
            <div className="flex flex-col gap-1 w-full max-w-[640px]">
                <div className="flex items-center space-x-2 rtl:space-x-reverse">
                    <span className="text-sm font-semibold text-gray-900 dark:text-white capitalize">
                        {message.role === 'user' ? 'You' : message.role}
                    </span>
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">{message.time}</span>
                </div>
                <div className={`flex flex-col leading-1.5 p-4 border-gray-200 ${backgroundColour} rounded-e-xl rounded-es-xl dark:bg-gray-700`}>
                    <p className="text-sm font-normal text-gray-900 dark:text-white">{message.messageText}</p>
                </div>
                {citations.length ? (
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">
                        <a
                            href="#"
                            className="color-blue underline"
                            onClick={(event) => {
                                event.preventDefault();
                                citationsRef.current?.classList.toggle('hidden');
                            }}
                        >
                            Why?
                        </a>{' '}
                        &nbsp;
                        <span>{message.processingTimeSeconds} seconds</span>
                        <div ref={citationsRef} className="mt-6 hidden">
                            <strong>Response was inferred from {citations.length} citations:</strong>
                            {(message.messageData as ICitation[]).map((citation: any) => (
                                <ol className="p-3 list-decimal">
                                    {citation.partitions.map((partition: any, index: number) => (
                                        <li key={index}>
                                            Source: <strong>{citation.sourceName}</strong> &nbsp;
                                            Partition: {partition.partitionNumber} &nbsp;
                                            Section: {partition.sectionNumber}
                                            <p className="m-3"><i>"{partition.text}"</i></p>
                                        </li>
                                    ))}
                                </ol>
                            ))}
                        </div>
                    </span>
                ) : (
                    <span></span>
                )}
            </div>
            {message.role === ConversationMessageRole.assistant && (
                <>
                    <button
                        id="dropdownMenuIconButton"
                        data-dropdown-toggle="dropdownDots"
                        data-dropdown-placement="bottom-start"
                        className="inline-flex self-center items-center p-2 text-sm font-medium text-center text-gray-900 bg-white rounded-lg hover:bg-gray-100 focus:ring-4 focus:outline-none dark:text-white focus:ring-gray-50 dark:bg-gray-900 dark:hover:bg-gray-800 dark:focus:ring-gray-600"
                        type="button"
                    >
                        <svg
                            className="w-4 h-4 text-gray-500 dark:text-gray-400"
                            aria-hidden="true"
                            xmlns="http://www.w3.org/2000/svg"
                            fill="currentColor"
                            viewBox="0 0 4 15"
                        >
                            <path d="M3.5 1.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 6.041a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 5.959a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Z" />
                        </svg>
                    </button>
                    <div id="dropdownDots" className="z-10 hidden bg-white divide-y divide-gray-100 rounded-lg shadow w-40 dark:bg-gray-700 dark:divide-gray-600">
                        <ul className="py-2 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="dropdownMenuIconButton">
                            <li>
                                <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">
                                    Yes, show me
                                </a>
                            </li>
                            <li>
                                <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">
                                    Not right now
                                </a>
                            </li>
                            <li>
                                <a
                                    href="#"
                                    className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white"
                                    onClick={stopServer}
                                >
                                    Stop server
                                </a>
                            </li>
                        </ul>
                    </div>
                </>
            )}
        </article>
    );
};

export default ConversationMessageGenericComponent;
