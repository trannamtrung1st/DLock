const apiUrl = {
  identity: {
    login: 'api/login',
    logout: 'api/logout/:userName'
  },
  field: {
    getAllFields: 'api/fields',
    changeMaintenanceStatus: 'api/fields/maintenance/:value'
  },
  booking: {
    createBooking: 'api/bookings',
    getBookings: 'api/bookings/:userName',
    getAllBookings: 'api/bookings',
  },
  user: {
    getAllUsers: 'api/users'
  },
  resetBookingData: 'api/restart',
  resetFieldData: 'api/restart',
  resetIdentityData: 'api/restart',
  changeProxyConfig: 'config/:configName',
  changeLockService: 'lock/:serviceName',
};

export {
  apiUrl
}