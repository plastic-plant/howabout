import { useSystemInfo } from './SystemService';

describe('SystemService', () => {
    beforeEach(() => {
        global.fetch = jest.fn();
    });

    it('should retrieve system info', () => {
        const expected = { cpuUsagePercentage: 75, memoryUsagePercentage: 75 } as SystemInfo;

        jest.spyOn(global, 'fetch').mockImplementationOnce(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(expected),
            } as Response)
        );

        const systemInfo = useSystemInfo();
        expect(systemInfo?.cpuUsagePercentage).toBe(expected.cpuUsagePercentage);
        expect(global.fetch).toHaveBeenCalledTimes(1);
    });
});
