import { useEffect, useState } from "react";
import EventMessageService from "../messages/EventMessageService";

export function useDocuments(): Record<string, IDocumentProperties[]> | undefined {
    const [documents, setDocuments] = useState<Record<string, IDocumentProperties[]>>();
    const { events } = EventMessageService();


    useEffect(() => {
        const updateDocumentGroupedByTags = async (): Promise<Record<string, IDocumentProperties[]>> => {
            const response = await fetch('api/listgroupedbytag');
            const data: Record<string, IDocumentProperties[]> = await response.json();
            setDocuments(data);
            return data;
        }
        updateDocumentGroupedByTags().catch(console.error);

        const handleDocumentChangedEvent = () => updateDocumentGroupedByTags().catch(console.error);
        events(handleDocumentChangedEvent, undefined);
    }, []);
    

    return documents;
};

export async function addDocument(file: Blob, tags: string[]): Promise<string> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('json', new Blob([JSON.stringify({ tags } as IDocumentAddRequest)], { type: 'application/json' }), 'request.json');
    try {
        const response = await fetch('/api/add', {
            method: 'POST',
            body: formData,
        });

        if (response.ok) {
            const data = await response.json();
            return data.message;
        } else {
            return 'Error uploading file.';
        }
    } catch (error) {
        return 'Error uploading file.';
    }
};

export interface IDocumentProperties {
    id: string;
    name: string;
    extension: string;
    originalPath: string;
    tags: string[];
    size: string;
}

export interface IDocumentAddRequest {
    tags: string[];
    urls: string[];
}
