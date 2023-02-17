import { FieldModel } from "@app/models/field.model";
import { apiUrl } from "@shared/constants/api.const";
import { appFetch, handleCommonError, handleCommonResponse } from "@shared/helpers/api.helper";
import { ApiResponseWrapper } from "@shared/models/api-response.model";

const fieldService: {
  changeMaintenanceStatus(fieldName: string, value: boolean): Promise<ApiResponseWrapper<any>>;
  resetData(): Promise<ApiResponseWrapper<any>>;
  getAllFields: () => Promise<ApiResponseWrapper<FieldModel[]>>;
} = {
  changeMaintenanceStatus(fieldName, value) {
    const path = apiUrl.field.changeMaintenanceStatus
      .replace(':value', `${value}`);
    const url = new URL(path, process.env.REACT_APP_FIELD_API_BASE);
    fieldName && url.searchParams.append('name', fieldName);

    return appFetch(url, {
      method: 'PUT',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  resetData() {
    const path = apiUrl.resetFieldData;
    const url = new URL(path, process.env.REACT_APP_FIELD_API_BASE);

    return appFetch(url, {
      method: 'POST',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  getAllFields() {
    const path = apiUrl.field.getAllFields;
    const url = new URL(path, process.env.REACT_APP_FIELD_API_BASE);

    return appFetch(url, {
      method: 'GET'
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  }
};

export default fieldService;
