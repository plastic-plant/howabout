import React, { useState, ChangeEvent, KeyboardEvent, MouseEvent } from 'react';

interface QuestionComponentProps { }

const QuestionComponent: React.FC<QuestionComponentProps> = () => {
    const [messageText, setMessageText] = useState<string>('');

    const handleMessageTextChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
        setMessageText(event.target.value);
    };

    const sendMessageOnEnterKey = async (event: KeyboardEvent<HTMLTextAreaElement>) => {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            const promise = askMessage(messageText);
            setMessageText('');
            await promise;
        }
    };

    const sendMessageOnButtonClick = async (event: MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        const promise = askMessage(messageText);
        setMessageText('');
        await promise;
    };

    const askMessage = async (message: string): Promise<string> => {
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


    return (
        <form>
            <div className="w-full mb-4 border border-gray-200 rounded-lg bg-gray-50 dark:bg-gray-700 dark:border-gray-600">

                <div className="relative px-4 py-2 bg-white rounded-t-lg dark:bg-gray-800">
                    <textarea id="comment" rows={4} value={messageText} onChange={handleMessageTextChange} onKeyPress={sendMessageOnEnterKey} className="w-full px-0 text-sm text-gray-900 bg-white border-0 border-transparent focus:border-transparent focus:ring-0 !outline-none" placeholder="Ask a question..."></textarea>
                </div>

                <div className="flex items-center justify-between px-3 py-2 border-t dark:border-gray-600">
                    <button type="submit" onClick={sendMessageOnButtonClick} className="inline-flex items-center py-2.5 px-4 text-xs font-medium text-center text-white bg-blue-700 rounded-lg focus:ring-4 focus:ring-blue-200 dark:focus:ring-blue-900 hover:bg-blue-800">
                        Send
                    </button>

                    <small><i>(ENTER to send, SHIFT + ENTER for new line)</i></small>

                    <div className="flex ps-0 space-x-1 rtl:space-x-reverse sm:ps-2">
                        <button type="button" className="inline-flex justify-center items-center p-2 text-gray-500 rounded cursor-pointer hover:text-gray-900 hover:bg-gray-100 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600">
                            <svg className="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 12 20">
                                <path stroke="currentColor" strokeLinejoin="round" strokeWidth="2" d="M1 6v8a5 5 0 1 0 10 0V4.5a3.5 3.5 0 1 0-7 0V13a2 2 0 0 0 4 0V6" />
                            </svg>
                            <span className="sr-only">Upload document</span>
                        </button>
                    </div>
                </div>
            </div>
        </form>
    );
};

export default QuestionComponent;
