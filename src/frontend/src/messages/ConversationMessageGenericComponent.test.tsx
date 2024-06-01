import { render, screen, fireEvent } from '@testing-library/react';
import ConversationMessageGenericComponent from './ConversationMessageGenericComponent';
import '@testing-library/jest-dom'

describe('ConversationMessageGenericComponent', () => {
  const message = {
    id: '1',
    role: 'user',
    time: '10:00 AM',
    messageText: 'Hello',
    messageData: [
      {
        partitions: [
          {
            text: 'Partition 1',
            partitionNumber: 1,
            sectionNumber: 1,
          },
          {
            text: 'Partition 2',
            partitionNumber: 2,
            sectionNumber: 2,
          },
        ],
        sourceName: 'Source 1',
      },
      {
        partitions: [
          {
            text: 'Partition 3',
            partitionNumber: 3,
            sectionNumber: 3,
          },
        ],
        sourceName: 'Source 2',
      },
    ],
    processingTimeSeconds: 5,
  };

  it('renders the message correctly', () => {
    render(<ConversationMessageGenericComponent message={message} />);

    expect(screen.getByText('You')).toBeInTheDocument();
    expect(screen.getByText('10:00 AM')).toBeInTheDocument();
    expect(screen.getByText('Hello')).toBeInTheDocument();
    expect(screen.getByText('Response was inferred from 2 citations:')).toBeInTheDocument();
    expect(screen.getByText('Partition 1')).toBeInTheDocument();
    expect(screen.getByText('Partition 2')).toBeInTheDocument();
    expect(screen.getByText('Partition 3')).toBeInTheDocument();
    expect(screen.getByText('Source 1')).toBeInTheDocument();
    expect(screen.getByText('Source 2')).toBeInTheDocument();
  });

  it('toggles citations visibility when "Why?" link is clicked', () => {
    render(<ConversationMessageGenericComponent message={message} />);

    const whyLink = screen.getByText('Why?');
    const citationsContainer = screen.getByTestId('citations-container');

    expect(citationsContainer).toHaveClass('hidden');

    fireEvent.click(whyLink);

    expect(citationsContainer).not.toHaveClass('hidden');

    fireEvent.click(whyLink);

    expect(citationsContainer).toHaveClass('hidden');
  });

  it('calls stopServer function when "Stop server" link is clicked', () => {
    const stopServerMock = jest.fn();
    const assistantMessage = {
      ...message,
      role: 'assistant',
    };

    render(<ConversationMessageGenericComponent message={assistantMessage} />);

    const stopServerLink = screen.getByText('Stop server');

    fireEvent.click(stopServerLink);

    expect(stopServerMock).toHaveBeenCalled();
  });
});
