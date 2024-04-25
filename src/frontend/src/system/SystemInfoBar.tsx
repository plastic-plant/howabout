import React from 'react';

interface Props {
    title: string;
    percentage: number;
    paused: boolean;
}

const SystemInfoBar: React.FC<Props> = ({ title, percentage, paused }) => {
    return (
        <div className={"mb-3 " + (paused ? 'systeminfo-paused' : '')}>
            <div className="flex justify-between mb-1">
                <span className="text-base font-medium text-blue-700 dark:text-white">{title}</span>
                <span className="text-sm font-medium text-blue-700 dark:text-white">{paused ? 'paused' : `${percentage}%`}</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                <div className="bg-blue-600 h-2.5 rounded-full systeminfo-transition" style={{ width: `${percentage}%` }}></div>
            </div>
        </div>
    );
};

export default SystemInfoBar;