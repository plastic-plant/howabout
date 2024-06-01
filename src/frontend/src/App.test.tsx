import '@testing-library/jest-dom'
import { render } from "@testing-library/react"
import App from "./App"

describe('App', () => {
    it("renders the main page", () => {
        render(<App />)
        expect(true).toBeTruthy()
    })

    test('renders SystemComponent', () => {
        const { getByTestId } = render(<App />);
        const systemComponent = getByTestId('system-component');
        expect(systemComponent).toBeInTheDocument();
    });

    test('renders DocumentUploadComponent', () => {
        const { getByTestId } = render(<App />);
        const documentUploadComponent = getByTestId('document-upload-component');
        expect(documentUploadComponent).toBeInTheDocument();
    });

    test('renders DocumentsOverviewComponent', () => {
        const { getByTestId } = render(<App />);
        const documentsOverviewComponent = getByTestId('documents-overview-component');
        expect(documentsOverviewComponent).toBeInTheDocument();
    });

    test('renders QuestionComponent', () => {
        const { getByTestId } = render(<App />);
        const questionComponent = getByTestId('question-component');
        expect(questionComponent).toBeInTheDocument();
    });

    test('renders ConversationComponent', () => {
        const { getByTestId } = render(<App />);
        const conversationComponent = getByTestId('conversation-component');
        expect(conversationComponent).toBeInTheDocument();
    });
});