import { render, fireEvent, act } from '@testing-library/react';
import SystemComponent from './SystemComponent';
import fetchMock from 'jest-fetch-mock';
import '@testing-library/jest-dom'

fetchMock.enableMocks();

describe('SystemComponent', () => {
    beforeEach(() => {
        fetchMock.resetMocks();
        jest.useFakeTimers();
    });

    afterAll(() => {
        jest.useRealTimers();
    });

    it('renders system bars with default props', async () => {
        fetchMock.mockResponseOnce(JSON.stringify({ cpuUsagePercentage: 50, memoryUsagePercentage: 60 }));

        const { getByText } = render(<SystemComponent />);

        await act(async () => {
            jest.advanceTimersByTime(2000);
        });

        expect(getByText('CPU')).toBeInTheDocument();
        expect(getByText('Memory')).toBeInTheDocument();
        expect(getByText('50%')).toBeInTheDocument();
        expect(getByText('60%')).toBeInTheDocument();
    });

    it('renders system bars with custom props', async () => {
        fetchMock.mockResponseOnce(JSON.stringify({ cpuUsagePercentage: 50, memoryUsagePercentage: 60 }));

        const { getByText } = render(<SystemComponent pause={true} intervalSeconds={5} shouldPauseWhenNotViewed={false} />);

        await act(async () => {
            jest.advanceTimersByTime(5000);
        });

        expect(getByText('CPU')).toBeInTheDocument();
        expect(getByText('Memory')).toBeInTheDocument();
        expect(getByText('50%')).toBeInTheDocument();
        expect(getByText('60%')).toBeInTheDocument();
    });

    xit('pauses system bars when paused prop is true', async () => {
        fetchMock.mockResponseOnce(JSON.stringify({ cpuUsagePercentage: 50, memoryUsagePercentage: 60 }));

        const { getByText, container } = render(<SystemComponent pause={true} />);

        await act(async () => {
            jest.advanceTimersByTime(2000);
        });

        const cpuBar = container.querySelector('.my-8:nth-child(1)');
        const memoryBar = container.querySelector('.my-8:nth-child(2)');

        expect(cpuBar).toHaveClass('systeminfo-paused');
        expect(memoryBar).toHaveClass('systeminfo-paused');
        expect(getByText('paused')).toBeInTheDocument();
    });

    it('handles visibility change correctly', async () => {
        fetchMock.mockResponseOnce(JSON.stringify({ cpuUsagePercentage: 50, memoryUsagePercentage: 60 }));

        const { container } = render(<SystemComponent />);

        await act(async () => {
            jest.advanceTimersByTime(2000);
        });

        Object.defineProperty(document, 'hidden', { value: true, writable: true });
        fireEvent(document, new Event('visibilitychange'));

        const cpuBar = container.querySelector('.my-8:nth-child(1)');
        const memoryBar = container.querySelector('.my-8:nth-child(2)');

        expect(cpuBar).toHaveClass('systeminfo-paused');
        expect(memoryBar).toHaveClass('systeminfo-paused');
    });
});
