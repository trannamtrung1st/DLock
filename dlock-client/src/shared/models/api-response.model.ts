export interface ApiResponse<T> {
  messages?: string[];
  data?: T;
}

export interface ApiResponseWrapper<T> {
  response: Response;
  apiData?: ApiResponse<T>;
}