import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import ConversationComponent from './ConversationComponent';
import '@testing-library/jest-dom'


describe('ConversationComponent', () => {
  test('renders loading message when messages are empty', () => {
    render(<ConversationComponent />);
    const loadingMessage = screen.getByText(/Loading.../i);
    expect(loadingMessage).toBeInTheDocument();
  });

  test('renders conversation messages', () => {
    const messages = [
      {
        id: 1,
        messageType: 'generic',
        role: 'user',
        content: 'Hello',
      },
      {
        id: 2,
        messageType: 'upload',
        role: 'bot',
        content: 'File uploaded',
      },
    ];
    render(<ConversationComponent />);
    const conversationMessages = screen.getAllByRole('section');
    expect(conversationMessages).toHaveLength(messages.length);
  });

  test('scrolls to last message after adding a new message', () => {
    render(<ConversationComponent />);
    const messagesEndRef = screen.getByTestId('messages-end-ref');
    const scrollToViewMock = jest.fn();
    Object.defineProperty(messagesEndRef, 'scrollIntoView', { value: scrollToViewMock });
    userEvent.click(screen.getByText(/Add Message/i));
    expect(scrollToViewMock).toHaveBeenCalled();
  });

  test('displays "Waiting for response..." message when last message is by user', () => {
    render(<ConversationComponent />);
    const waitingMessage = screen.getByText(/Waiting for response.../i);
    expect(waitingMessage).toBeInTheDocument();
  });
});
