// tailwind.config.js
module.exports = {
  // Specify the paths to all of your template files
  content: [
    './src/**/*.{html,js,jsx,ts,tsx,vue}', // Adjust according to your project structure
    './public/index.html',
  ],
  theme: {
    extend: {
      colors: {
        // IBM Carbon Gray Palette
        gray: {
          10: '#161616',
          20: '#383838',
          30: '#4F4F4F',
          40: '#666666',
          50: '#7A7A7A',
          60: '#919191',
          70: '#A6A6A6',
          80: '#BDBDBD',
          90: '#D3D3D3',
          100: '#FFFFFF',
        },
        // IBM Carbon Blue Palette
        blue: {
          10: '#0F62FE',
          20: '#1570EF',
          30: '#1883FC',
          40: '#4F83CC',
          50: '#0062FF',
          60: '#0353E9',
          70: '#0052CC',
          80: '#003D99',
          90: '#002766',
          100: '#001D4A',
        },
        // IBM Carbon Green Palette
        green: {
          10: '#008DA6',
          20: '#00A19D',
          30: '#00BFBC',
          40: '#5D9C59',
          50: '#13B886',
          60: '#11A07F',
          70: '#0E8C6A',
          80: '#0B6B54',
          90: '#0A5A46',
          100: '#0A4C3A',
        },
        // IBM Carbon Red Palette
        red: {
          10: '#DA1E28',
          20: '#CD2D3B',
          30: '#BF1B27',
          40: '#A61C22',
          50: '#DC3618',
          60: '#BF241F',
          70: '#9A1C1A',
          80: '#780D16',
          90: '#5A0E14',
          100: '#3E0C12',
        },
        // IBM Carbon Yellow Palette
        yellow: {
          10: '#FDD13A',
          20: '#FEC514',
          30: '#FEC514',
          40: '#FEC514',
          50: '#FEC514',
          60: '#FEC514',
          70: '#FEC514',
          80: '#FEC514',
          90: '#FEC514',
          100: '#FEC514',
        },
        // Add more IBM Carbon color palettes as needed
      },
      // Optional: Customize other theme aspects (e.g., fonts, spacing) here
      fontFamily: {
        sans: ['IBM Plex Sans', 'sans-serif'],
        serif: ['IBM Plex Serif', 'serif'],
        mono: ['IBM Plex Mono', 'monospace'],
      },
    },
  },
  plugins: [
    // Add any Tailwind CSS plugins you need here
    // Example: require('@tailwindcss/forms'),
  ],
  darkMode: 'class', // Enable dark mode if needed (optional)
};
