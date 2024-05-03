

import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';

class EventMessageService {
    private connection: HubConnection;
    public events: (
        onDocumentChangedEvent?: () => void,
        onMessageAddedEvent?: (message: IConversationMessage) => void
    ) => void
    static instance: EventMessageService;

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl("/hubs/eventMessageHub", HttpTransportType.LongPolling)
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => document.write(err));
        this.events = (onDocumentChangedEvent, onMessageAddedEvent) => {
            onDocumentChangedEvent && this.connection.on("DocumentChangedEvent", () => onDocumentChangedEvent());
            onMessageAddedEvent && this.connection.on("MessageAddedEvent", (message) => onMessageAddedEvent(message));
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