import { useSystemInfo } from './system/SystemService';
import './App.css';
import DocumentUpload from './documents/DocumentUpload';
import SystemInfoBar from './system/SystemInfoBar';
import Conversation from './messages/Conversation';
import ChatEditor from './messages/ChatEditor';
import DocumentsOverview from './documents/DocumentsOverview';

function App() {

    const systemInfo = useSystemInfo();

    return (
        <div>
            <div className="flex h-screen bg-white border-r border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                <aside className="w-96 px-6 py-2">                    
                    <SystemInfoBar title="CPU" percentage={systemInfo?.cpuUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                    <SystemInfoBar title="Memory" percentage={systemInfo?.memoryUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                    <DocumentUpload />
                    <DocumentsOverview />
                </aside>
                <main className="w-full h-screen px-6 py-2 flex flex-col-reverse">
                    <section className="w-full m-1">                    
                        <ChatEditor />
                    </section>
                    <section className="flex-grow thinscrollbar">
                        <Conversation />
                    </section>
                    <header className="flex h-44 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
                        <nav className="self-end w-4/6 px-4 py-8 inline-block">
                            <span className="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap text-orange uppercase">How<span className="text-lightorange">about</span> your documents?</span>
                        </nav>
                    </header>
                </main>
            </div>
        </div>

    );
}

export default App;