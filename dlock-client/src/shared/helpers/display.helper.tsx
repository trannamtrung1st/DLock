import { routeMap } from '@shared/constants/routes.const';
import { MenuItemVM } from '@shared/types/menu-item-vm.type';
import { Empty } from 'antd';
import { DefaultOptionType } from 'antd/es/select';
import { MenuInfo } from 'rc-menu/lib/interface';
import { Link } from 'react-router-dom';
import {
  HomeOutlined,
  CalendarOutlined,
  SettingOutlined,
  FileOutlined
} from '@ant-design/icons';
import { RouteSideMenu } from '@shared/types/route-side-menu.type';
import { RouteInfo } from '@shared/types/route-info.type';

const getSideMenuInfo = (routeLink: string): RouteSideMenu | undefined => {
  switch (routeLink) {
    case routeMap.home.link: return {
      sideMenuIcon: <HomeOutlined />,
    }
    case routeMap.booking.link: return {
      sideMenuIcon: <CalendarOutlined />,
    }
    case routeMap.sharing.link: return {
      sideMenuIcon: <FileOutlined />,
    }
    case routeMap.admin.link: return {
      sideMenuIcon: <SettingOutlined />,
    }
    default: return;
  }
}

const buildSideMenuItem = ({
  link, title
}: RouteInfo, children?: MenuItemVM[]): MenuItemVM => {
  const { sideMenuHasSub, sideMenuIcon, sideMenuLink } = getSideMenuInfo(link) || {};
  const menuItem = getMenuItem(title || '', link, (
    sideMenuHasSub ? title : <Link to={sideMenuLink || link}>{title}</Link>
  ), sideMenuIcon, children);
  return menuItem;
}

const getMenuItem = (
  title: string,
  key: string,
  label?: React.ReactNode,
  icon?: React.ReactNode,
  children?: MenuItemVM[],
  onClick?: (menu: MenuInfo) => void,
): MenuItemVM => {
  return {
    title,
    onClick,
    key,
    icon,
    children,
    label: label || title
  };
}

const isScrollToBottom = (element: Element, offset: number = 7) =>
  element.scrollHeight - element.scrollTop <= element.clientHeight + offset;

const generateEmpty = (description: string) => <Empty description={description} image={Empty.PRESENTED_IMAGE_SIMPLE} />;

const avatarColors = [
  '#eb3434', '#f56a00', '#ffa200', '#d3d622',
  '#62eb00', '#009945', '#00e6e2', '#005ad9',
  '#7948c2', '#e803fc', '#ff0099', '#6e0018'
];
const randomAvatarColor = (value: string) => {
  let hash = 0;
  if (value.length === 0) return '#f5f5f5';
  for (var i = 0; i < value.length; i++) {
    hash = value.charCodeAt(i) + ((hash << 5) - hash);
    hash = hash & hash;
  }
  hash = ((hash % avatarColors.length) + avatarColors.length) % avatarColors.length;
  return avatarColors[hash];
}

const defaultFilterOption = (input: string, option?: DefaultOptionType) =>
  (option?.label ?? '').toString().toLowerCase().includes(input.toLowerCase());

export type ScrollToXySettings = {
  x: number, y: number, behavior?: ScrollBehavior
};
export type ScrollToElementSettings = {
  element: Element, behavior?: ScrollBehavior,
  offsetX?: number, offsetY?: number
};
const scrollTo = (args: ScrollToXySettings | ScrollToElementSettings) => {
  let finalX: number, finalY: number, finalBehavior: ScrollBehavior | undefined;
  if ('element' in args) {
    const { element, behavior, offsetX, offsetY } = args;
    const rect = element.getBoundingClientRect();
    finalX = rect.left + window.scrollX - (offsetX || 0);
    finalY = rect.top + window.scrollY - (offsetY || 0);
    finalBehavior = behavior;
  } else {
    const { x, y, behavior } = args;
    finalX = x; finalY = y; finalBehavior = behavior;
  }
  window.scrollTo({ behavior: finalBehavior, left: finalX, top: finalY });
}

export {
  getMenuItem,
  generateEmpty,
  randomAvatarColor,
  defaultFilterOption,
  isScrollToBottom,
  scrollTo,
  buildSideMenuItem
}