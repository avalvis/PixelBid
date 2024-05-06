import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
    "node_modules/flowbite-react/**/*.{js,ts,jsx,tsx}"
  ],
  theme: {
    extend: {
      fontFamily: {
        'custom': ['"Arial Black", Gadget, sans-serif']
      },
      backgroundColor: {
        'custom-yellow': '#F3CA52'
      },
      boxShadow: {
        'custom': '0px 4px 8px rgba(0, 0, 0, 0.2)'
      },
      borderRadius: {
        'custom': '10px'
      },
      padding: {
        'custom': '10px'
      },
      scale: {
        '105': '1.05'
      },
      backgroundImage: {
        "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
        "gradient-conic": "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
      },
    },
  },
  variants: {
    extend: {
      scale: ['hover']
    },
  },
  corePlugins: {
    aspectRatio: false,
  },
  plugins: [
    require('@tailwindcss/aspect-ratio'),
    require('flowbite/plugin')
  ],
};

export default config;