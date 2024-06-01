import React, { ChangeEvent, useState } from 'react';

export interface IDocumentAddRequest {
    tags: string[];
    urls: string[];
}

const DocumentUploadComponent: React.FC = () => {
    const [isUploading, setUploading] = useState(false);

    const addDocument = async (file: Blob, tags: string[]): Promise<string> => {
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

    const uploadDocument = async (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files[0]) {
            const tags = ['Documents'];
            setUploading(true);
            await addDocument(event.target.files[0], tags);
            setUploading(false);
        }
    };

    return (
        <div className="my-12 border-2 border-gray-300 border-dashed rounded-lg bg-gray-50 dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
            <form className={isUploading ? 'opacity-50' : ''}>
                <div className="flex items-center justify-center w-full">
                    <label className="relative flex flex-col items-center justify-center w-full h-64 cursor-pointer bg-gray-50 dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                        <div className="flex flex-col items-center justify-center pt-5 pb-6">
                            <svg className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 16">
                                <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2" />
                            </svg>
                            <p className="mb-2 text-sm text-gray-500 dark:text-gray-400"><span className="font-semibold">Click to upload</span> or drag and drop</p>
                            <p className="text-xs text-gray-500 dark:text-gray-400">Word documents, PDF or text files</p>
                        </div>
                        <input disabled={isUploading} type="file" onChange={uploadDocument} className="absolute top-0 left-0 w-full h-full opacity-0 cursor-pointer" />
                    </label>
                </div>
            </form>
        </div>
    );
};

export default DocumentUploadComponent;