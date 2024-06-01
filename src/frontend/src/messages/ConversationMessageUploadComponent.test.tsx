import { render } from '@testing-library/react';
import ConversationMessageUploadComponent, { DocumentIcon } from './ConversationMessageUploadComponent';
import { IConversationMessage, IDocumentProperties } from './Models';

describe('ConversationMessageUploadComponent', () => {
  const message: IConversationMessage = {
    id: '1',
    messageData: {
      name: 'file',
      extension: 'pdf',
      size: '1.5MB',
      originalPath: 'https://example.com/document.pdf',
    },
    time: '10:00 AM',
    processingTimeSeconds: 5,
  };

  it('renders the user profile picture', () => {
    const { getByAltText } = render(<ConversationMessageUploadComponent message={message} />);
    const userProfilePicture = getByAltText('User profile picture');
    expect(userProfilePicture).toBeInTheDocument();
  });

  it('renders the username', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const username = getByText('You');
    expect(username).toBeInTheDocument();
  });

  it('renders the message time', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const messageTime = getByText('10:00 AM');
    expect(messageTime).toBeInTheDocument();
  });

  it('renders the document name', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const documentName = getByText('Document upload');
    expect(documentName).toBeInTheDocument();
  });

  it('renders the document extension', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const documentExtension = getByText('pdf');
    expect(documentExtension).toBeInTheDocument();
  });

  it('renders the document size', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const documentSize = getByText('1.5MB');
    expect(documentSize).toBeInTheDocument();
  });

  it('renders the document download link', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const downloadLink = getByText('Download');
    expect(downloadLink).toBeInTheDocument();
    expect(downloadLink).toHaveAttribute('href', 'https://example.com/document.pdf');
  });

  it('renders the processing time', () => {
    const { getByText } = render(<ConversationMessageUploadComponent message={message} />);
    const processingTime = getByText('5 seconds');
    expect(processingTime).toBeInTheDocument();
  });
});

describe('DocumentIcon', () => {
  it('renders the correct icon for doc and docx files', () => {
    const document: IDocumentProperties = {
      name: 'file',
      extension: 'doc',
      size: '1.5MB',
      originalPath: 'https://example.com/document.doc',
    };
    const { getByTestId } = render(<DocumentIcon document={document} />);
    const docIcon = getByTestId('doc-icon');
    expect(docIcon).toBeInTheDocument();
  });

  it('renders the correct icon for txt files', () => {
    const document: IDocumentProperties = {
      name: 'file',
      extension: 'txt',
      size: '1.5MB',
      originalPath: 'https://example.com/document.txt',
    };
    const { getByTestId } = render(<DocumentIcon document={document} />);
    const txtIcon = getByTestId('txt-icon');
    expect(txtIcon).toBeInTheDocument();
  });

  it('renders the correct icon for pdf files', () => {
    const document: IDocumentProperties = {
      name: 'file',
      extension: 'pdf',
      size: '1.5MB',
      originalPath: 'https://example.com/document.pdf',
    };
    const { getByTestId } = render(<DocumentIcon document={document} />);
    const pdfIcon = getByTestId('pdf-icon');
    expect(pdfIcon).toBeInTheDocument();
  });

  it('renders a default icon for unknown file types', () => {
    const document: IDocumentProperties = {
      name: 'file',
      extension: 'unknown',
      size: '1.5MB',
      originalPath: 'https://example.com/document.unknown',
    };
    const { getByTestId } = render(<DocumentIcon document={document} />);
    const defaultIcon = getByTestId('default-icon');
    expect(defaultIcon).toBeInTheDocument();
  });
});
