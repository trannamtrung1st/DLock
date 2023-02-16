import { flatRoutes } from '@shared/constants/routes.const';
import { matchPath } from "react-router-dom";

export const getCurrentRoutes = (pathname: string, end: boolean) => {
  const selection = flatRoutes.filter(route => !!matchPath({
    path: route.link,
    end
  }, pathname));
  return selection;
};
