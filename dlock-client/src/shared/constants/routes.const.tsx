import { RouteInfo, StructuredRouteInfo } from "@shared/types/route-info.type";

export const routeMap: {
  root: RouteInfo,
  home: RouteInfo,
  booking: RouteInfo,
  admin: RouteInfo,
  login: RouteInfo
} = {
  root: {
    link: '/'
  },
  home: {
    link: '/home',
    title: 'Home'
  },
  booking: {
    link: '/booking',
    title: 'Booking'
  },
  admin: {
    link: '/admin',
    title: 'Admin'
  },
  login: {
    link: '/login',
    title: 'Login',
  }
};

export const flatRoutes = (() => {
  const queue: RouteInfo[] = Object.values(routeMap);
  const flatRoutes: RouteInfo[] = [];
  while (queue.length) {
    const currentItem: RouteInfo | undefined = queue.shift();
    const children = currentItem?.c;
    if (children) {
      const childrenList = Object.values(children);
      (currentItem as StructuredRouteInfo).childrenList = childrenList;
      childrenList.forEach(child => queue.push(child));
    }
    currentItem && flatRoutes.push(currentItem);
  }
  return flatRoutes;
})();