
import React, { useState } from 'react';

function DocumentUpload() {
    const [file, setFile] = useState<File | null>(null);
    const [message, setMessage] = useState<string>('');

    const updateFile = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setFile(e.target.files[0]);
        }
    };

    const submitFile = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!file) {
            setMessage('Please select a file');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);
        formData.append('json', new Blob([JSON.stringify({ tags: ['tag4'] } as DocumentAddRequest)], { type: 'application/json' }), 'request.json');

        try {
            const response = await fetch('/api/add', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                const data = await response.json();
                setMessage(data.message);
            } else {
                setMessage('Error uploading file');
            }
        } catch (error) {
            setMessage('Error uploading file');
        }
    };

    return (
        <div>
            <div className="flex items-center justify-center w-full">
                <label className="relative flex flex-col items-center justify-center w-full h-64 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                    <div className="flex flex-col items-center justify-center pt-5 pb-6">
                        <svg className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 16">
                            <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2" />
                        </svg>
                        <p className="mb-2 text-sm text-gray-500 dark:text-gray-400"><span className="font-semibold">Click to upload</span> or drag and drop</p>
                        <p className="text-xs text-gray-500 dark:text-gray-400">DOC, DOCX, PDF, TXT</p>
                    </div>
                    <input type="file" className="absolute top-0 left-0 w-full h-full opacity-0 cursor-pointer" />
                </label>
            </div>
            <form className="mt-6" onSubmit={submitFile}>
                <input type="file" onChange={updateFile} /><br />
                <input type="text" name="tag" placeholder="tag1" /><br />
                <input type="text" name="tag" placeholder="tag2" /><br />
                <button type="submit">Upload</button><br />
            </form>
            <p>{message}</p>
        </div>
    );
}

export default DocumentUpload;

interface DocumentAddRequest {
    tags: string[];
    urls: string[];
}