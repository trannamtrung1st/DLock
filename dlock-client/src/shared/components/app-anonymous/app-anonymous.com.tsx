import { GlobalContext } from '@app/contexts/global.context';
import { routeMap } from '@shared/constants/routes.const';
import React, { FC, PropsWithChildren, useContext } from 'react';
import { Navigate } from 'react-router-dom';

interface Props extends PropsWithChildren {
}

const AppAnonymous: FC<Props> = ({ children }) => {
  const { userName } = useContext(GlobalContext) || {};
  const isLoggedIn = !!userName;
  if (isLoggedIn) {
    return <Navigate to={routeMap.root.link} />
  } else {
    return <>{children}</>;
  }
};

export default AppAnonymous;
