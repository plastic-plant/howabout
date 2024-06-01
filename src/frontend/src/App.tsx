import React from 'react';
import './App.css';
import QuestionComponent from './ask/QuestionComponent';
import DocumentUploadComponent from './documents/DocumentUploadComponent';
import DocumentsOverviewComponent from './documents/DocumentsOverviewComponent';
import ConversationComponent from './messages/ConversationComponent';
import SystemComponent from './system/SystemComponent';

const App: React.FC = () => {    
    return (
        <div className="flex h-screen bg-white border-r border-gray-200 dark:bg-gray-800 dark:border-gray-700">
            <aside className="w-96 px-6 py-2">
                <SystemComponent />
                <DocumentUploadComponent />
                <DocumentsOverviewComponent />
            </aside>
            <main className="w-full h-screen px-6 py-2 flex flex-col-reverse">
                <section className="w-full m-1">
                    <QuestionComponent />
                </section>
                <section className="flex-grow thinscrollbar">
                    <ConversationComponent />
                </section>
                <header className="flex h-44 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                    <nav className="self-end w-4/6 px-4 py-8 inline-block">
                        <span className="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-orange uppercase">
                            How<span className="text-lightorange">about</span> your documents?
                        </span>
                    </nav>
                </header>
            </main>
        </div>
    );
};

export default App;
