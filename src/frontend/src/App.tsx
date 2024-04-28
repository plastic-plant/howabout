import { useSystemInfo } from './system/SystemService';
import './App.css';
import DocumentUpload from './documents/DocumentUpload';
import SystemInfoBar from './system/SystemInfoBar';
import Logo from './menu/Logo';
import Chat from './messages/Conversation';
import ChatEditor from './messages/ChatEditor';
import DocumentUploadedMessage from './messages/DocumentUploadedMessage';
import WelcomeHelpMessage from './messages/WelcomeHelpMessage';
import DocumentsOverview from './documents/DocumentsOverview';

function App() {

    const systemInfo = useSystemInfo();

    return (
        <div>
            <header className="flex w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                <aside className="w-2/6 px-1 py-8 inline-block">
                    <Logo />
                </aside>
                <nav className="w-4/6 px-4 py-8 inline-block">
                    <span className="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-orange uppercase">Let's <span className="text-lightorange">chat about</span> your documents</span>
                </nav>
            </header>

            <div className="flex h-screen bg-white border-r border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                <aside className="w-2/6 px-6 py-2">                    
                    <SystemInfoBar title="CPU" percentage={systemInfo?.cpuUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                    <SystemInfoBar title="Memory" percentage={systemInfo?.memoryUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                    <DocumentUpload />
                    <DocumentsOverview />
                </aside>
                <main className="w-4/6 px-6 py-2 relative">

                    <WelcomeHelpMessage />
                    <Chat />
                    <DocumentUploadedMessage />

                    <section className="absolute bottom-0 left-0 w-full">                    
                        <ChatEditor />
                    </section>

                </main>
            </div>
        </div>

    );
}

export default App;