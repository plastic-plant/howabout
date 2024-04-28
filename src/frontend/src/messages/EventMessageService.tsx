

import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';

class EventMessageService {
    private connection: HubConnection;
    public events: (
        onDocumentAddedEvent: (message: string) => void,
        onDocumentRemovedEvent: (message: string) => void
    ) => void
    static instance: EventMessageService;

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl("/hubs/eventMessageHub", HttpTransportType.LongPolling)
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => document.write(err));
        this.events = (onDocumentAddedEvent) => {
            this.connection.on("DocumentAddedEvent", (message) => onDocumentAddedEvent(message));
            this.connection.on("DocumentRemovedEvent", (message) => onDocumentAddedEvent(message));
        };
    }
    
    public static getInstance(): EventMessageService {
        if (!EventMessageService.instance) {
            EventMessageService.instance = new EventMessageService();
        }
        return EventMessageService.instance;
    }
}
export default EventMessageService.getInstance;