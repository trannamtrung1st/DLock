const path = require('path');
const webpack = require('webpack');

module.exports = {
  typescript: {
    enableTypeChecking: true,
  },
  webpack: {
    plugins: [],
    alias: {
      '@': path.resolve(__dirname, 'src/'),
      '@app': path.resolve(__dirname, 'src/app'),
      '@shared': path.resolve(__dirname, 'src/shared'),
      'style-variables': path.resolve(__dirname, 'src/styles/_variables.scss'),
    },
  },
};

export { }

