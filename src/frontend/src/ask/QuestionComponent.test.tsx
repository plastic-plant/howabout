import { render, fireEvent, waitFor } from '@testing-library/react';
import QuestionComponent from './QuestionComponent';
import '@testing-library/jest-dom'


describe('QuestionComponent', () => {
  test('should update messageText state when textarea value changes', () => {
    const { getByPlaceholderText } = render(<QuestionComponent />);
    const textarea = getByPlaceholderText('Ask a question...');
    fireEvent.change(textarea, { target: { value: 'Test message' } });
    expect(textarea.value).toBe('Test message');
  });

  test('should call askMessage function when Enter key is pressed', async () => {
    const mockAskMessage = jest.fn();
    const { getByPlaceholderText } = render(<QuestionComponent />);
    const textarea = getByPlaceholderText('Ask a question...');
    fireEvent.keyPress(textarea, { key: 'Enter', shiftKey: false });
    await waitFor(() => {
      expect(mockAskMessage).toHaveBeenCalled();
    });
  });

  test('should call askMessage function when Send button is clicked', async () => {
    const mockAskMessage = jest.fn();
    const { getByText } = render(<QuestionComponent />);
    const sendButton = getByText('Send');
    fireEvent.click(sendButton);
    await waitFor(() => {
      expect(mockAskMessage).toHaveBeenCalled();
    });
  });

  test('should reset messageText state after sending message', async () => {
    const { getByPlaceholderText, getByText } = render(<QuestionComponent />);
    const textarea = getByPlaceholderText('Ask a question...');
    const sendButton = getByText('Send');
    fireEvent.change(textarea, { target: { value: 'Test message' } });
    fireEvent.click(sendButton);
    await waitFor(() => {
      expect(textarea.value).toBe('');
    });
  });
});
