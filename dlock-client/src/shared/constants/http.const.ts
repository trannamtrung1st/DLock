const commonHeaders = {
  cacheControl: {
    name: 'Cache-Control',
    noCache: 'no-cache',
    maxAge: 'max-age={maxAge}'
  },
}

const contentTypes = {
  image: {
    png: 'image/png'
  }
};

export {
  commonHeaders,
  contentTypes
}