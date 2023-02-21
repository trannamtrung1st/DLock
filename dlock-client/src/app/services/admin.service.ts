import { apiUrl } from "@shared/constants/api.const";
import { appFetch, handleCommonError, handleCommonResponse } from "@shared/helpers/api.helper";
import { ApiResponseWrapper } from "@shared/models/api-response.model";

const adminService: {
  changeProxyConfig(configName: string): Promise<ApiResponseWrapper<any>>;
  changeLockService(serviceName: string): Promise<ApiResponseWrapper<any>>;
} = {
  changeLockService: (serviceName) => {
    const path = apiUrl.changeLockService.replace(':serviceName', serviceName);

    let url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);
    return appFetch(url, {
      method: 'PUT',
    }).then(handleCommonResponse())
      .catch(handleCommonError());
  },

  changeProxyConfig(configName) {
    const path = apiUrl.changeProxyConfig.replace(':configName', configName);
    const url = new URL(path, process.env.REACT_APP_BOOKING_API_BASE);

    return appFetch(url, {
      method: 'PUT',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  }
};

export default adminService;
