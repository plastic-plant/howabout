import { render, fireEvent, waitFor, screen } from '@testing-library/react';
import DocumentUploadComponent from './DocumentUploadComponent';
import '@testing-library/jest-dom'

describe('DocumentUploadComponent', () => {
  test('should render the component', () => {
    render(<DocumentUploadComponent />);
    const uploadButton = screen.getByText('Click to upload');
    expect(uploadButton).toBeInTheDocument();
  });

  test('should call addDocument function when a file is uploaded', async () => {
    const mockAddDocument = jest.fn();
    const mockFile = new File(['test file'], 'test.txt', { type: 'text/plain' });
    const mockChangeEvent = {
      target: {
        files: [mockFile],
      },
    };

    render(<DocumentUploadComponent />);
    const fileInput = screen.getByLabelText('Click to upload or drag and drop');
    fireEvent.change(fileInput, mockChangeEvent);

    await waitFor(() => {
      expect(mockAddDocument).toHaveBeenCalledWith(mockFile, ['Documents']);
    });
  });

  test('should display error message when file upload fails', async () => {
    const mockAddDocument = jest.fn().mockRejectedValue('Error uploading file.');

    render(<DocumentUploadComponent />);
    const fileInput = screen.getByLabelText('Click to upload or drag and drop');
    fireEvent.change(fileInput, { target: { files: [new File(['test file'], 'test.txt', { type: 'text/plain' })] } });

    await waitFor(() => {
      const errorMessage = screen.getByText('Error uploading file.');
      expect(errorMessage).toBeInTheDocument();
    });
  });

  test('should display success message when file upload is successful', async () => {
    const mockAddDocument = jest.fn().mockResolvedValue('File uploaded successfully.');

    render(<DocumentUploadComponent />);
    const fileInput = screen.getByLabelText('Click to upload or drag and drop');
    fireEvent.change(fileInput, { target: { files: [new File(['test file'], 'test.txt', { type: 'text/plain' })] } });

    await waitFor(() => {
      const successMessage = screen.getByText('File uploaded successfully.');
      expect(successMessage).toBeInTheDocument();
    });
  });
});
