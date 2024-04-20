/** @type {import('tailwindcss').Config} */
export default {
    content: [
        './src/**/*.{js,jsx,ts,tsx}',
        './src/node_modules/flowbite-react/lib/esm/**/*.js',
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('flowbite/plugin'),
    ],
}

