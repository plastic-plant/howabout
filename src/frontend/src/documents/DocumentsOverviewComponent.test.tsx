import { render, screen } from '@testing-library/react';
import DocumentsOverviewComponent from './DocumentsOverviewComponent';
import '@testing-library/jest-dom'


describe('DocumentsOverviewComponent', () => {
  test('renders loading message when documents are undefined', () => {
    render(<DocumentsOverviewComponent />);
    const loadingMessage = screen.getByText('Loading...');
    expect(loadingMessage).toBeInTheDocument();
  });

  test('renders document tags and names when documents are defined', () => {
    const documents = {
      tag1: [
        {
          id: '1',
          name: 'Document 1',
          extension: 'pdf',
          originalPath: '/path/to/document1.pdf',
          tags: ['tag1'],
          size: '1 MB',
        },
      ],
      tag2: [
        {
          id: '2',
          name: 'Document 2',
          extension: 'docx',
          originalPath: '/path/to/document2.docx',
          tags: ['tag2'],
          size: '2 MB',
        },
        {
          id: '3',
          name: 'Document 3',
          extension: 'txt',
          originalPath: '/path/to/document3.txt',
          tags: ['tag2'],
          size: '500 KB',
        },
      ],
    };

    render(<DocumentsOverviewComponent />);
    const loadingMessage = screen.queryByText('Loading...');
    expect(loadingMessage).not.toBeInTheDocument();

    const tag1Button = screen.getByText('tag1');
    expect(tag1Button).toBeInTheDocument();

    const tag2Button = screen.getByText('tag2');
    expect(tag2Button).toBeInTheDocument();

    const document1 = screen.getByText('Document 1');
    expect(document1).toBeInTheDocument();

    const document2 = screen.getByText('Document 2');
    expect(document2).toBeInTheDocument();

    const document3 = screen.getByText('Document 3');
    expect(document3).toBeInTheDocument();
  });
});
