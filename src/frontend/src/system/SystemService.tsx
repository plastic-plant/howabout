import { useEffect, useState } from "react";

export function useSystemInfo(pause: boolean = false, intervalSeconds: number = 2, shouldPauseWhenNotViewed: boolean = true): SystemInfo | undefined {
    const [systemInfo, setSystemInfo] = useState<SystemInfo>();

    useEffect(() => {

        let interval: NodeJS.Timeout;

        const updateSystemInfo = async () => {
            if (shouldPauseWhenNotViewed && document.hidden) {
                setSystemInfo({ ...systemInfo, paused: true } as SystemInfo);
            } else {
                setSystemInfo(await getSystemInfo());
            }
        };

        if (!pause) {
            interval = setInterval(updateSystemInfo, intervalSeconds * 1000);
        }

        updateSystemInfo();

        return () => {
            clearInterval(interval);
        };
    }, [pause]);

    const getSystemInfo = async (): Promise<SystemInfo> => {
        const response = await fetch('system/metrics');
        const data: SystemInfo = await response.json();

        // Pause the view when both CPU and memory usage are 0.
        if (!data.cpuUsagePercentage && !data.memoryUsagePercentage) {
            return { ...data, paused: true };
        }

        // Ignore the CPU usage if temporarily not available.
        if (!data.cpuUsagePercentage || data.cpuUsagePercentage == 0) {
            return { ...systemInfo, memoryUsagePercentage: data.memoryUsagePercentage, paused: false };
        }

        return { ...data, paused: false };
    }

    return systemInfo;
};
