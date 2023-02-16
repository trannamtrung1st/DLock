import { BookingModel } from "@app/models/booking.model";
import { CreateBookingModel } from "@app/models/create-booking.model";
import { apiUrl } from "@shared/constants/api.const";
import { appFetch, appFetchJson, handleCommonError, handleCommonResponse } from "@shared/helpers/api.helper";
import { ApiResponseWrapper } from "@shared/models/api-response.model";

const bookingService: {
  resetData(): Promise<ApiResponseWrapper<any>>;
  creatBooking: (model: CreateBookingModel) => Promise<ApiResponseWrapper<BookingModel>>;
  getBookings: (userName: string) => Promise<ApiResponseWrapper<BookingModel[]>>;
  getAllBookings: () => Promise<ApiResponseWrapper<BookingModel[]>>;
} = {
  resetData() {
    const path = apiUrl.resetBookingData;
    const url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);

    return appFetch(url, {
      method: 'POST',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  getBookings(userName) {
    const path = apiUrl.booking.getBookings.replace(':userName', userName);
    const url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);

    return appFetch(url, {
      method: 'GET',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  getAllBookings() {
    const path = apiUrl.booking.getAllBookings;
    const url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);

    return appFetch(url, {
      method: 'GET',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  creatBooking(model) {
    const path = apiUrl.booking.createBooking;
    const url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);

    return appFetchJson(url, {
      method: 'POST',
      body: JSON.stringify(model)
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  }
};

export default bookingService;
