
import React, { useState } from 'react';
import { useDocuments, addDocument } from './DocumentService';

function DocumentUpload() {
    const [tags, setTags] = useState<string[]>([]);
    const [tagValue, setTagValue] = useState('');

    const changeTag = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTagValue(event.target.value);
    };

    const addTag = () => {
        if (tagValue.trim() !== '') {
            setTags([...tags, tagValue]);
            setTagValue('');
        }
    };

    const deleteTag = (index: number) => {
        const newTags = tags.filter((_, i) => i !== index);
        setTags(newTags);
    };

    const documents = useDocuments();

    const uploadDocument = async (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files[0]) {
            const tags = ['Documents'];
            await addDocument(event.target.files[0], tags);
        }
    };

    return (
        <div className="my-12">
            {documents ? (
                <div className="border-2 border-gray-300 border-dashed rounded-lg bg-gray-50 dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                    <form>
                        <div className="flex items-center justify-center w-full">
                            <label className="relative flex flex-col items-center justify-center w-full h-64 cursor-pointer bg-gray-50 dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                                <div className="flex flex-col items-center justify-center pt-5 pb-6">
                                    <svg className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 16">
                                        <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2" />
                                    </svg>
                                    <p className="mb-2 text-sm text-gray-500 dark:text-gray-400"><span className="font-semibold">Click to upload</span> or drag and drop</p>
                                    <p className="text-xs text-gray-500 dark:text-gray-400">Word documents, PDF or text files</p>
                                </div>
                                <input type="file" onChange={uploadDocument} className="absolute top-0 left-0 w-full h-full opacity-0 cursor-pointer" />
                            </label>
                        </div>
                    </form>
                </div>
                ) : (
                    <p>Loading...</p>
                )}
        </div>
    );
}

export default DocumentUpload;
