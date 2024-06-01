import React, { Component } from 'react';

export interface SystemInfo {
    // SystemInfo API response from backend.
    cpuUsagePercentage?: number;
    memoryUsagePercentage?: number;

    // Additional field for frontend.
    paused: boolean;
}

interface SystemProps {
    pause?: boolean;
    intervalSeconds?: number;
    shouldPauseWhenNotViewed?: boolean;
}

interface SystemState {
    systemInfo?: SystemInfo;
}

class SystemComponent extends Component<SystemProps, SystemState> {
    static defaultProps = {
        pause: false,
        intervalSeconds: 2,
        shouldPauseWhenNotViewed: true,
    };

    interval: NodeJS.Timeout | null = null;

    constructor(props: SystemProps) {
        super(props);
        this.state = {
            systemInfo: undefined,
        };
    }

    async componentDidMount() {
        this.updateSystemInfo();
        if (!this.props.pause) {
            this.interval = setInterval(this.updateSystemInfo, this.props.intervalSeconds! * 1000);
        }
        document.addEventListener('visibilitychange', this.handleVisibilityChange);
    }

    componentWillUnmount() {
        if (this.interval) {
            clearInterval(this.interval);
        }
        document.removeEventListener('visibilitychange', this.handleVisibilityChange);
    }

    handleVisibilityChange = () => {
        if (this.props.shouldPauseWhenNotViewed && document.hidden) {
            this.setState((prevState) => ({
                systemInfo: { ...prevState.systemInfo, paused: true },
            }));
        } else {
            this.updateSystemInfo();
        }
    };

    updateSystemInfo = async () => {
        const systemInfo = await this.getSystemInfo();
        this.setState({ systemInfo });
    };

    getSystemInfo = async (): Promise<SystemInfo> => {
        const response = await fetch('/api/metrics');
        const data: SystemInfo = await response.json();

        if (!data.cpuUsagePercentage && !data.memoryUsagePercentage) {
            return { ...data, paused: true };
        }

        if (!data.cpuUsagePercentage || data.cpuUsagePercentage === 0) {
            return { ...this.state.systemInfo, memoryUsagePercentage: data.memoryUsagePercentage, paused: false };
        }

        return { ...data, paused: false };
    };

    render() {
        const { systemInfo } = this.state;

        return (
            <>
                <Bar title="CPU" percentage={systemInfo?.cpuUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
                <Bar title="Memory" percentage={systemInfo?.memoryUsagePercentage ?? 0} paused={systemInfo?.paused ?? true} />
            </>
        );
    }
}

interface SystemBarProps {
    title: string;
    percentage: number;
    paused: boolean;
}

const Bar: React.FC<SystemBarProps> = ({ title, percentage, paused }) => {
    return (
        <div className={"my-8 " + (paused ? 'systeminfo-paused' : '')}>
            <div className="flex justify-between mb-1">
                <span className="text-base font-medium text-gray">{title}</span>
                <span className="text-sm font-medium text-lightorange">{paused ? 'paused' : `${percentage}%`}</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                <div className="bg-orange h-2.5 rounded-full systeminfo-transition" style={{ width: `${percentage}%` }}></div>
            </div>
        </div>
    );
};

export default SystemComponent;
