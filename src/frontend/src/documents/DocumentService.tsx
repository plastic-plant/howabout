import { useEffect, useState } from "react";

export function useDocuments(): Record<string, IDocumentProperties[]> | undefined {
    const [documents, setDocuments] = useState<Record<string, IDocumentProperties[]>>();

    useEffect(() => {
        const updateDocumentGroupedByTags = async (): Promise<Record<string, IDocumentProperties[]>> => {
            const response = await fetch('api/listgroupedbytag');
            const data: Record<string, IDocumentProperties[]> = await response.json();
            setDocuments(data);
            return data;
        }

        updateDocumentGroupedByTags()
            .catch(console.error);
    }, []);
    

    return documents;
};

export interface IDocumentProperties {
    id: string;
    name: string;
    originalPath: string;
    tags: string[];
}
