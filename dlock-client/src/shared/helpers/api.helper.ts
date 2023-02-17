import { showErrorMessage } from "@shared/helpers/messages.helper";
import { ApiResponse, ApiResponseWrapper } from "@shared/models/api-response.model";
import { handledError, isHandledError } from "./exception.helper";
import { antdMessage } from "@shared/components/antd-functions";

const handleCommonResponse = (silent: boolean = false) => async (response: Response): Promise<any> => {
  switch (response.status) {
    case 204: {
      const apiResponseWrapper: ApiResponseWrapper<ApiResponse<any>> = {
        response
      };
      return apiResponseWrapper;
    }
    case 200:
    case 400: {
      try {
        const apiData: ApiResponse<any> = await response.json();
        const apiResponseWrapper: ApiResponseWrapper<ApiResponse<any>> = {
          apiData,
          response
        };
        return apiResponseWrapper;
      } catch (e) {
        console.log(response);
        console.log(e);
        !silent && showErrorMessage();
      }
      break;
    }
    case 401: {
      !silent && antdMessage.warning('Chưa xác thực');
      break;
    }
    case 403: {
      !silent && antdMessage.warning('Không có quyền truy cập');
      break;
    }
    case 404: {
      !silent && antdMessage.warning('Không tìm thấy');
      break;
    }
    default: {
      !silent && showErrorMessage();
      break;
    }
  }

  // Error cases only
  console.log(response);
  throw handledError;
};

const handleCommonError = (silent: boolean = false) => (reason: any) => {
  if (!isHandledError(reason)) {
    !silent && showErrorMessage();
    console.log(reason);
  }
  throw handledError;
};

const appFetch = (input: RequestInfo | URL,
  init?: RequestInit
) => {
  const headers: any = init?.headers || {};
  return fetch(input, {
    ...init,
    headers: headers
  })
};

const appFetchJson = (input: RequestInfo | URL,
  init?: RequestInit
) => {
  const headers: any = init?.headers || {};
  headers['Content-Type'] = 'application/json';
  return fetch(input, {
    ...init,
    headers: headers
  })
};

export {
  handleCommonResponse,
  handleCommonError,
  appFetch,
  appFetchJson
}