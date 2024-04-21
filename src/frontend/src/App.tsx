import { useSystemInfo } from './system/SystemService';
import './App.css';

function App() {

    const systemInfo = useSystemInfo();

    const stopServer = async () => {
        const response = await fetch('http://localhost:5153/configuration/stop');
        if (response.ok) {
            window.alert('Couldn\'t stop the server.');
        } else {
            window.alert('Stopped. Bye.');
        }
    };

    const howabout = <span className="howabout">How<i>about</i></span>;
    const systemInfoBar = (title: string, percentage: number, paused: boolean) =>
        <div className={"my-6 " + (paused ? 'systeminfo-paused' : '')}>
            <div className="flex justify-between mb-1">
                <span className="text-base font-medium text-blue-700 dark:text-white">{title}</span>
                <span className="text-sm font-medium text-blue-700 dark:text-white">{paused ? 'paused' : `${percentage}%` }</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                <div className="bg-blue-600 h-2.5 rounded-full systeminfo-transition" style={{ width: `${percentage}%` }}></div>
            </div>
        </div>;
    const chat =
        <div className="flex items-start gap-2.5 my-6">
            <img className="w-8 h-8 rounded-full" src="/assistant.jpg" alt="Assistant profile picture" />
            <div className="flex flex-col gap-1 w-full max-w-[320px]">
                <div className="flex items-center space-x-2 rtl:space-x-reverse">
                    <span className="text-sm font-semibold text-gray-900 dark:text-white">Assistant</span>
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">18:02</span>
                </div>
                <div className="flex flex-col leading-1.5 p-4 border-gray-200 bg-gray-100 rounded-e-xl rounded-es-xl dark:bg-gray-700">
                    <p className="text-sm font-normal text-gray-900 dark:text-white">Welcome. {howabout} started succesfully. I see you haven't setup providers for embedding and conversation in my appsettings.json configuration. <b>Can I help you setup?</b></p>
                </div>
                <span className="text-sm font-normal text-gray-500 dark:text-gray-400">Ready</span>
            </div>
            <button id="dropdownMenuIconButton" data-dropdown-toggle="dropdownDots" data-dropdown-placement="bottom-start" className="inline-flex self-center items-center p-2 text-sm font-medium text-center text-gray-900 bg-white rounded-lg hover:bg-gray-100 focus:ring-4 focus:outline-none dark:text-white focus:ring-gray-50 dark:bg-gray-900 dark:hover:bg-gray-800 dark:focus:ring-gray-600" type="button">
                <svg className="w-4 h-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 4 15">
                    <path d="M3.5 1.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 6.041a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 5.959a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Z" />
                </svg>
            </button>
            <div id="dropdownDots" className="z-10 hidden bg-white divide-y divide-gray-100 rounded-lg shadow w-40 dark:bg-gray-700 dark:divide-gray-600">
                <ul className="py-2 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="dropdownMenuIconButton">
                    <li>
                        <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Yes, show me</a>
                    </li>
                    <li>
                        <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Not right now</a>
                    </li>
                    <li>
                        <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white" onClick={stopServer}>Stop server</a>
                    </li>
                </ul>
            </div>
        </div>;

    return (
        <div>
           <header className="sticky top-0 z-50"></header>
            <main className="relative">
                <h1>{howabout}?</h1>
                {systemInfoBar("CPU", systemInfo?.cpuUsagePercentage ?? 0, systemInfo?.paused ?? true)}
                {systemInfoBar("Memory", systemInfo?.memoryUsagePercentage ?? 0, systemInfo?.paused ?? true)}
                {chat}
            </main>
           <footer></footer>
        </div>
    );
}

export default App;