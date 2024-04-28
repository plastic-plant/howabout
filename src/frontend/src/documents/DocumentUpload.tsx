
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
            const tags = ['default'];
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

                        <div className="flex m-2">
                            <button id="dropdown-button" data-dropdown-toggle="dropdown" className="flex-shrink-0 z-10 inline-flex items-center py-2.5 px-4 text-sm font-medium text-center text-gray-500 bg-gray-100 border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-200" type="button">Choose a tag <svg className="w-2.5 h-2.5 ms-2.5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 10 6">
                                <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m1 1 4 4 4-4" />
                            </svg></button>
                            <div id="dropdown" className="z-10 hidden bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700">
                                <ul className="py-2 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="dropdown-button">
                                    {Object.keys(documents).length === 0 ? (
                                        <li>
                                            <a href="#" className="block px-4 py-2 hover:bg-gray-100">Default</a>
                                        </li>
                                    ) : (
                                        Object.keys(documents).map((tag) => (
                                            <li>
                                                <a href="#" className="block px-4 py-2 hover:bg-gray-100">{tag}</a>
                                            </li>
                                        ))
                                    )}
                                </ul>
                            </div>

                            <div className="relative w-full">
                                <input type="search" value={tagValue} onChange={changeTag} className="block p-2.5 w-full z-20 text-sm text-gray-900 bg-gray-50 rounded-e-lg rounded-s-gray-100 rounded-s-2 border border-gray-300 focus:ring-lightorange focus:border-lightorange" placeholder="or add a new group tag..." required />
                                <button type="submit" onClick={addTag} className="absolute top-0 end-0 p-2.5 h-full text-sm font-medium text-white bg-orange rounded-e-lg border border-orange hover:bg-darkorange focus:ring-4 focus:outline-none focus:ring-lightorange">
                                    <svg className="w-6 h-6 text-gray-800 " aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                        <path stroke="white" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 12h14m-7 7V5" />
                                    </svg>
                                </button>
                            </div>
                        </div>

                        <div className="tags mx-3">
                            {tags.map((tag, index) => (
                                <span key={tag} className="inline-flex items-center px-2 py-1 me-2 text-sm font-medium text-gray-800 bg-gray-100 rounded dark:bg-gray-700 dark:text-gray-300">
                                    {tag}
                                    <button type="button" className="inline-flex items-center p-1 ms-2 text-sm text-gray-400 bg-transparent rounded-sm hover:bg-gray-200 hover:text-gray-900 dark:hover:bg-gray-600 dark:hover:text-gray-300" data-dismiss-target="#badge-dismiss-dark" aria-label="Remove">
                                        <svg className="w-2 h-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                                            <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                                        </svg>
                                        <span className="sr-only" onClick={() => deleteTag(index)}>Remove</span>
                                    </button>
                                </span>
                            ))}
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
