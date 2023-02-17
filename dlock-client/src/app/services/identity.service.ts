import { UserModel } from "@app/models/user.model";
import { apiUrl } from "@shared/constants/api.const";
import { appFetch, appFetchJson, handleCommonError, handleCommonResponse } from "@shared/helpers/api.helper";
import { ApiResponseWrapper } from "@shared/models/api-response.model";

const USER_NAME_STORAGE_KEY = '@dl/userName';

const identityService: {
  resetData(): Promise<ApiResponseWrapper<any>>;
  getCurrentUserName: () => string | null;
  saveUserName: (userName: string) => void;
  clearUserName: () => void;
  login: (userName: string) => Promise<ApiResponseWrapper<any>>;
  logout: (userName: string) => Promise<ApiResponseWrapper<any>>;
  getAllUsers: () => Promise<ApiResponseWrapper<UserModel[]>>;
} = {
  getAllUsers() {
    const path = apiUrl.user.getAllUsers;
    const url = new URL(path, process.env.REACT_APP_IDENTITY_API_BASE);

    return appFetch(url, {
      method: 'GET',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  resetData() {
    const path = apiUrl.resetIdentityData;
    const url = new URL(path, process.env.REACT_APP_IDENTITY_API_BASE);

    return appFetch(url, {
      method: 'POST',
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },

  getCurrentUserName() {
    const userName = localStorage.getItem(USER_NAME_STORAGE_KEY);
    return userName;
  },

  saveUserName(userName) {
    localStorage.setItem(USER_NAME_STORAGE_KEY, userName);
    return userName;
  },

  clearUserName() {
    localStorage.removeItem(USER_NAME_STORAGE_KEY);
  },

  async login(userName) {
    const path = apiUrl.identity.login;
    const url = new URL(path, process.env.REACT_APP_IDENTITY_API_BASE);

    return appFetchJson(url, {
      method: 'POST',
      body: JSON.stringify({ userName }),
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  },
  async logout(userName) {
    const path = apiUrl.identity.logout.replace(':userName', userName);
    const url = new URL(path, process.env.REACT_APP_IDENTITY_API_BASE);

    return appFetch(url, {
      method: 'POST'
    }).then(handleCommonResponse())
      .catch(handleCommonError())
  }
};

export default identityService;
