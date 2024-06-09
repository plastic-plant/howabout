import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { SystemInfo } from '../system/SystemComponent';

export class SystemInfoService {
    public connection: HubConnection;
    static instance: SystemInfoService;

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl("/hubs/systemInfoHub", HttpTransportType.LongPolling)
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => document.write(err));
    }
    
    public static getInstance(): SystemInfoService {
        if (!SystemInfoService.instance) {
            SystemInfoService.instance = new SystemInfoService();
        }
        return SystemInfoService.instance;
    }

    public getMetrics(): Promise<SystemInfo> {
        return this.connection.invoke("getMetrics");
    }
}

export default SystemInfoService.getInstance;