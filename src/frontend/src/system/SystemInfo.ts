interface SystemInfo {
    // SystemInfo API response from backend.
    cpuUsagePercentage?: number;
    memoryUsagePercentage?: number;

    // Additional field for frontend.
    paused: boolean;
}