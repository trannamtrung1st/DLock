import React, { useEffect, useState, useMemo, useContext } from 'react';
import { Outlet, useLocation } from "react-router-dom";
import {
  LogoutOutlined
} from '@ant-design/icons';
import { Col, Dropdown, Row, Space } from 'antd';
import { Layout, Menu } from 'antd';
import './app-layout.com.scss';
import { Typography } from 'antd';
import { getCurrentRoutes } from '@shared/helpers/routing.helper';
import { MenuItemVM } from '@shared/types/menu-item-vm.type';
import { buildSideMenuItem, getMenuItem } from '@shared/helpers/display.helper';
import { routeMap } from '@shared/constants/routes.const';
import { appMessages } from '@shared/constants/messages.const';
import AppSpin from '../app-spin';
import { GlobalContext } from '@app/contexts/global.context';
import { isAdmin } from '@shared/constants/accounts.const';

const { Header, Content, Footer, Sider } = Layout;
const { Title } = Typography;

const AppLayout = () => {
  const { pathname } = useLocation();
  const { userName, logout } = useContext(GlobalContext) || {};
  const [sideMenuSelection, setSideMenuSelection] = useState<{
    items: MenuItemVM[],
    currentItem?: MenuItemVM
  }>({ items: [], currentItem: undefined });

  const sideMenuItems = useMemo(() => {
    const items = [
      buildSideMenuItem(routeMap.home),
      buildSideMenuItem(routeMap.booking),
      buildSideMenuItem(routeMap.sharing)
    ];
    if (userName && isAdmin(userName)) {
      items.push(buildSideMenuItem(routeMap.admin));
    }
    return items;
  }, [userName]);
  const flatMenuItems = useMemo(() => {
    const flatMenuItems: MenuItemVM[] = [];
    const queue = [...sideMenuItems];
    while (queue.length) {
      const currentItem: MenuItemVM | undefined = queue.shift();
      const children = currentItem?.children;
      if (children) {
        children.forEach(child => queue.push(child));
      }
      currentItem && flatMenuItems.push(currentItem);
    }
    return flatMenuItems;
  }, [sideMenuItems]);
  const currentRoute = useMemo(() => getCurrentRoutes(pathname, true)[0], [pathname]);
  const matchedRoutes = useMemo(() => currentRoute
    ? (currentRoute.customRoutesPath || getCurrentRoutes(pathname, false))
    : [], [pathname, currentRoute]);

  useEffect(() => {
    const menuSelection = flatMenuItems
      .filter(item => matchedRoutes.some(route => route.link === item.key));
    setSideMenuSelection({
      items: menuSelection,
      currentItem: menuSelection[menuSelection.length - 1]
    });
  }, [matchedRoutes, flatMenuItems]);

  const defaultSideMenuOpenKeys = useMemo(() => {
    return matchedRoutes.map(item => item.link);
  }, [matchedRoutes]);

  const sideMenuSelectedKeys = useMemo(() => {
    return sideMenuSelection.items.map(item => item.key);
  }, [sideMenuSelection]);

  const profileDropdownItems: MenuItemVM[] = useMemo(() => ([
    getMenuItem('Đăng xuất', 'logout', undefined, <LogoutOutlined />, undefined, logout)
  ]), [logout]);

  return (
    <AppSpin spinning={false}>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider theme="light">
          <div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />
          <Menu theme="light"
            defaultSelectedKeys={[]}
            selectedKeys={sideMenuSelectedKeys}
            defaultOpenKeys={defaultSideMenuOpenKeys}
            mode="inline"
            items={sideMenuItems} />
        </Sider>
        <Layout className="site-layout">
          <Header style={{ height: 47, padding: 0 }}>
            <Row style={{ height: 47, lineHeight: '100%' }} justify="end" align="middle">
              <Col flex="none" style={{ padding: '0 8px' }}>
                <Space size={"middle"}>
                  <Dropdown menu={{ items: profileDropdownItems }} placement="bottomRight">
                    <Space>
                      {userName ? `Hi ${userName}` : appMessages.common.ellipsis}
                    </Space>
                  </Dropdown>
                </Space>
              </Col>
            </Row>
          </Header>
          <Content style={{ margin: '16px', display: 'flex', flexDirection: 'column' }}>
            <Title level={2} style={{ marginTop: 0 }}>
              {currentRoute?.title}
            </Title>
            <Outlet />
          </Content>
          <Footer style={{ textAlign: 'center' }}>Ant Design ©2018 Created by Ant UED</Footer>
        </Layout>
      </Layout>
    </AppSpin>
  );
};

export default AppLayout;
