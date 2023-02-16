/// <reference types="react-scripts" />

declare namespace NodeJS {
  interface ProcessEnv {
    readonly REACT_APP_AUTHOR: string;
    readonly REACT_APP_IDENTITY_API_BASE: string;
    readonly REACT_APP_FIELD_API_BASE: string;
    readonly REACT_APP_BOOKING_API_BASE: string;
  }
}