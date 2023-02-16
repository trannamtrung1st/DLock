const rawColors = {
  lightGrey: '#f5f5f5',
  red: {
    normal: 'red',
  },
  yellow: {
    lighter: '#fff6ab',
    light: '#ffed4d',
    normal: '#decd35',
    dark: '#ad9c00'
  },
  green: {
    normal: '#198754',
    light: '#0bd600'
  },
  black: {
    normal: '#000',
    lighter: 'rgba(0, 0, 0, 0.88)'
  }
};

const colors = {
  ...rawColors,
  success: rawColors.green.light,
  danger: rawColors.red.normal
};

export {
  colors
}