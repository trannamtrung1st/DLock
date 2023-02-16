import { GlobalContext } from '@app/contexts/global.context';
import { identityService } from '@app/services';
import React, { FC, PropsWithChildren, useState, useMemo } from 'react';

interface Props extends PropsWithChildren {
}

const AppGlobalHandler: FC<Props> = ({ children }) => {
  const currentUserName = identityService.getCurrentUserName();
  const [userName, setUserName] = useState<string | null | undefined>(currentUserName);
  const logout = useMemo(() => () => {
    if (!userName || !setUserName) return;
    identityService.logout(userName).then(() => {
      identityService.clearUserName();
      setUserName(undefined);
    });
  }, [userName, setUserName]);

  return <GlobalContext.Provider value={{
    userName,
    setUserName,
    logout
  }}>
    {children}
  </GlobalContext.Provider>;
};

export default AppGlobalHandler;
