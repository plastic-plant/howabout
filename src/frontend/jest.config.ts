export default {
    preset: 'ts-jest',
    setupFiles: ['./jest.setup.ts'],
    testEnvironment: 'jest-environment-jsdom',
    transform: {
        "^.+\\.tsx?$": "ts-jest"
        // process `*.tsx` files with `ts-jest`
    },
    moduleNameMapper: {
        "\\.(css|less|sass|scss)$": "<rootDir>/test/__ mocks __/styleMock.js", // or go with 'identity-obj-proxy'
        '\\.(gif|ttf|eot|svg|png|jpg|pdf|doc|docx|txt)$': '<rootDir>/test/__ mocks __/fileMock.js',
    },
}