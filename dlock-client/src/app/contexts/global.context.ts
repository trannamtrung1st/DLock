import React from "react";

export interface GlobalContextType {
  userName?: string | null;
  setUserName: (userName?: string | null) => void;
  logout: () => void;
}

export const GlobalContext = React.createContext<GlobalContextType | undefined>(undefined)
