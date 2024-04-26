import { useSystemInfo } from './system/SystemService';
import './App.css';
import DocumentUpload from './documents/DocumentUpload';
import SystemInfoBar from './system/SystemInfoBar';
import Logo from './menu/Logo';
import Chat from './messages/Chat';
import ChatEditor from './messages/ChatEditor';
import DocumentUploadedMessage from './messages/DocumentUploadedMessage';
import WelcomeHelpMessage from './messages/WelcomeHelpMessage';
import DocumentsOverview from './documents/DocumentsOverview';

function App() {

    const systemInfo = useSystemInfo();

    return (
        <div>
            <header className="flex w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                <aside className="w-2/6 px-6 py-2 inline-block">
                    <SystemInfoBar title="CPU" percentage={systemInfo?.cpuUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                    <SystemInfoBar title="Memory" percentage={systemInfo?.memoryUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                </aside>
                <nav className="w-4/6 px-6 inline-block">
                    <Logo />
                </nav>
            </header>

            <div className="flex h-screen bg-white border-r border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                <aside className="w-2/6 m-3">
                    <DocumentUpload />
                    <DocumentsOverview />
                </aside>
                <main className="w-4/6 relative">

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