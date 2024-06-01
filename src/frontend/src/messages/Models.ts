export interface IConversationMessage {
    id: string;
    messageType: string | ConversationMessageType;
    role: string | ConversationMessageRole;
    time: string;
    messageText: string;
    messageData: IDocumentProperties | ICitation[];
    processingTimeSeconds: number;
}

export enum ConversationMessageType {
    none = "none",
    welcomeSetupRequired = "welcomeSetupRequired", // with the option to help stop and setup the server settings.
    welcomeUploadRequired = "welcomeUploadRequired", // with the option to have a quick example test upload.
    welcomeReady = "welcomeReady", // with an invitation to start a conversation.
    documentChange = "documentChange", // with a document icon and file details.
    conversation = "conversation" // for regular request and response messages with model.
}

export enum ConversationMessageRole {
    none = "none",
    system = "system",
    assistant = "assistant",
    user = "user"
}

export interface IDocumentProperties {
    id?: string;
    name?: string;
    extension: string;
    originalPath?: string;
    tags: string[];
    size: string;
}

export interface ICitation {
    link: string;
    index: string;
    documentId: string;
    fileId: string;
    sourceContentType: string;
    sourceName: string;
    sourceUrl: string;
    partitions: IPartition[];
}

export interface IPartition {
    text: string;
    relevance: number;
    partitionNumber: number;
    sectionNumber: number;
    lastUpdate: string;
    tags: string[];
}