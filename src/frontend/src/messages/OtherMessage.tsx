import { useRef } from "react";

const stopServer = async () => {
    const response = await fetch('http://localhost:5153/configuration/stop');
    if (response.ok) {
        window.alert('Couldn\'t stop the server.');
    } else {
        window.alert('Stopped. Bye.');
    }
};

export default function OtherMessage({ message }: { message: ConversationMessage }) {
    const citationsRef = useRef<null | HTMLDivElement>(null)
    const roleProfileImage = `/${message.role}.png`;
    const backgroundcolor = message.role === 'user' ? 'bg-lightorange' : 'bg-gray-100';
    const partitionsText: string[] = message.messageData && (message.messageData as any[])
        .flatMap((citation: { partitions: any[]; }) => citation.partitions
            .map(partition => partition.text)
        ) || [];
    
    return (
        <article key={message.id} className="flex items-start gap-2.5 my-6">
            <img className="w-8 h-8 rounded-full" src={roleProfileImage} />
            <div className="flex flex-col gap-1 w-full max-w-[640px]">
                <div className="flex items-center space-x-2 rtl:space-x-reverse">
                    <span className="text-sm font-semibold text-gray-900 dark:text-white capitalize">{message.role}</span>
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">{message.time}</span>
                </div>
                <div className={`flex flex-col leading-1.5 p-4 border-gray-200 ${backgroundcolor} rounded-e-xl rounded-es-xl dark:bg-gray-700`}>
                    <p className="text-sm font-normal text-gray-900 dark:text-white">{message.messageText}</p>
                </div>
                { partitionsText.length ? (
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">
                        <a href="#" className="color-blue underline" onClick={(event) => { event.preventDefault(); citationsRef.current?.classList.toggle('hidden'); }}>Why?</a> &nbsp; 
                        <i>(inferred from {partitionsText?.length} citations)</i>
                        <div ref={citationsRef} className="p-3 hidden">
                            {message.messageData.map((citation: any) => (
                                <ol className="list-decimal">
                                    {citation.partitions.map((partition: any, index: number) => (
                                        <li key={index}>
                                            Source: <strong>{citation.sourceName}</strong> &nbsp;
                                            Partition: {partition.partitionNumber} &nbsp;
                                            Section: {partition.sectionNumber}
                                            <p className="m-3"><i>"{partition.text}"</i></p></li>
                                    ))}
                                </ol>
                            ))}
                        </div>
                    </span>
                ) : (
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">Ready</span>
                )}
            </div>
        </article>
  );
}