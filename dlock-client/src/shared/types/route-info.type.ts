export interface RouteInfo {
  link: string;
  customRoutesPath?: RouteInfo[];
  title?: string;
  c?: { [key: string]: RouteInfo };
};

export interface StructuredRouteInfo extends RouteInfo {
  childrenList?: StructuredRouteInfo[];
}