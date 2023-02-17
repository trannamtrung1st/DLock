import { MenuInfo } from 'rc-menu/lib/interface';

export interface MenuItemVM {
  title: string;
  key: string;
  icon: React.ReactNode;
  children?: MenuItemVM[],
  label: React.ReactNode;
  onClick?: (menu: MenuInfo) => void;
};