import '@testing-library/jest-dom'
import { EventMessageService } from './EventMessageService';
import { IConversationMessage, IDocumentProperties } from '../messages/Models';

xdescribe('EventMessageService', () => {
  let eventMessageService: EventMessageService;

  beforeEach(() => {
    eventMessageService = EventMessageService.getInstance();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should create a new instance of EventMessageService', () => {
    expect(eventMessageService).toBeInstanceOf(EventMessageService);
  });

  it('should start the connection', async () => {
    const startSpy = jest.spyOn(eventMessageService.connection, 'start');

    await eventMessageService.connection.start();

    expect(startSpy).toHaveBeenCalled();
  });

  it('should call onDocumentChangedEvent when DocumentChangedEvent is received', () => {
    const onDocumentChangedEvent = jest.fn();
    const message = 'DocumentChangedEvent';

    eventMessageService.events(onDocumentChangedEvent, undefined);
    eventMessageService.connection.invoke('DocumentChangedEvent', message);

    expect(onDocumentChangedEvent).toHaveBeenCalled();
  });

  it('should call onMessageAddedEvent with the message when MessageAddedEvent is received', () => {
    const onMessageAddedEvent = jest.fn();
    const message: IConversationMessage = {
      id: '123',
      messageType: 'onDocumentChanged',
      role: 'assistant',
      time: '20:12',
      messageText: 'Added a document for you.',
      messageData: {} as IDocumentProperties,
      processingTimeSeconds: 2,
    };

    eventMessageService.events(undefined, onMessageAddedEvent);
    eventMessageService.connection.invoke('MessageAddedEvent', message);

    expect(onMessageAddedEvent).toHaveBeenCalledWith(message);
  });
});
