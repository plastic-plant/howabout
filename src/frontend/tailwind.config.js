/** @type {import('tailwindcss').Config} */
export default {
    content: [
        './src/**/*.{js,jsx,ts,tsx}'
    ],
    theme: {
        extend: {
            colors: {
                orange: '#FFA500',
                darkorange: '#ff8c00',
                lightorange: '#ffd580',
            },
        }
    },
    plugins: [
    ],
    safelist: [{
            pattern: /(bg|text|border|ring)-(orange|darkorange|lightorange).*/
        },{
            pattern: /(mt|mb|mr|ml|my|mx|px|py|pt|pb|pl|pr)-[0-9]+/
        },
        {
            pattern: /flex-.*/
        },
        {
            pattern: /(bottom|right|top|left)-[0-9]+/
        },
        {
            pattern: /(w|h)-[0-9]+/
        }
    ]
}
// bg-orange-600